using Microsoft.AspNetCore.Identity;

namespace quiz_game.Models
{
    public class AuthResponse
    {

        public List<string>? Errors { get; set; }

        public bool? Success { get; set; }

        public IdentityUser? User { get; set; }

        public string Token { get; set; }

    }
}
