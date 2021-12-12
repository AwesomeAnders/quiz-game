
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using quiz_game.Models;
using Microsoft.Extensions.Options;
using quiz_game.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace quiz_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;


        public AuthenticationController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] UserRegistration userRegistration)
        {
         
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(userRegistration.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use."
                        },
                        Success = false
                    }); ; 
                }

                var newUser = new IdentityUser() { Email = userRegistration.Email, UserName = userRegistration.Email };
                var isCreated = await _userManager.CreateAsync(newUser, userRegistration.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);

                    return Ok(new AuthResponse()
                    {
                       Success = true,
                       User = newUser,
                       Token = jwtToken
                    });
                }

                return BadRequest(new AuthResponse()
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });
            }

            return BadRequest(new AuthResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(userLogin.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLogin.Password);

                if (!isCorrect)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new AuthResponse()
                {
                    Success = true,
                    Token = jwtToken,
                    User = existingUser
                });
            }

            return BadRequest(new AuthResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }


        [Authorize]
        [HttpPost("validate")]
        public async Task<ActionResult<AuthResponse>> Validate()
        {
            return Ok();

        }

        private string GenerateJwtToken(IdentityUser user)
        {
         
            var jwtTokenHandler = new JwtSecurityTokenHandler();

        
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
