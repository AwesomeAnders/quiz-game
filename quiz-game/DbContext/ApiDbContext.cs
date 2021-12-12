using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using quiz_game.Models;

namespace quiz_game.DbContext
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOption> QuestionOptions { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {

        }
    }
}
