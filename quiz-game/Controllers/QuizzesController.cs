
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using quiz_game.DbContext;
using quiz_game.Models;

namespace quiz_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public QuizzesController(ApiDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {

            return await _context.Quizzes
                .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.QuestionOptions)
                .ToListAsync();


        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(long id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(long id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Attach(quiz);

                IEnumerable<EntityEntry> unchangedEntities = _context.ChangeTracker.Entries().Where(x => x.State == EntityState.Unchanged);

                foreach (EntityEntry ee in unchangedEntities)
                {
                    ee.State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(long id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var populatedQuiz = _context.Quizzes
                .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.QuestionOptions).First();
               

            _context.Quizzes.Remove(populatedQuiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(long id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}
