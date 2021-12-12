using System.ComponentModel.DataAnnotations;

namespace quiz_game.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
