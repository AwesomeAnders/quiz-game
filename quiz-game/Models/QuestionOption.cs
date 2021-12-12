using System.ComponentModel.DataAnnotations;

namespace quiz_game.Models
{
    public class QuestionOption
    {

        public long Id { get; set; }


        public string Text { get; set; }


        public bool IsCorrect { get; set; }

    }

}
