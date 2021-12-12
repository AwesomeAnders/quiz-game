using System.ComponentModel.DataAnnotations;

namespace quiz_game.Models
{
    public class Question
    {

        public long Id { get; set; }

        public string Description { get; set; }

        public virtual List<QuestionOption> QuestionOptions { get; set; }


    }
}
