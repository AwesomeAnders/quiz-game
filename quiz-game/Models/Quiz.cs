
using System.ComponentModel.DataAnnotations;

namespace quiz_game.Models { 
    public class Quiz

    {

        public long Id { get; set; }

        public string Titel { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public virtual List<Question> Questions { get; set; }




    }
}
