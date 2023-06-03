using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Converters;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITodoRepository _todoRepository;
        private readonly ITodoItemConverter _todoItemConverter;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings, ITodoRepository todoRepository, ITodoItemConverter todoItemConverter)
        {
            _userManager = userManager;
            _todoRepository = todoRepository;
            _todoItemConverter = todoItemConverter;
            _jwtSettings = jwtSettings.Value;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var tokenString = this.GenerateToken(user);

            return Ok(new { Token = tokenString, user.UserName, user.Email });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return Unauthorized(new { description = "Invalid credentials." });
            }

            var tokenString = GenerateToken(user);

            var todos = _todoItemConverter.ConvertToDto(await _todoRepository.GetAllAsync(user.Id));

            return Ok(new { Token = tokenString, user.UserName, user.Email, Todos = todos });
        }

        [NonAction]
        public string GenerateToken(Models.ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.UtcNow.AddHours(168),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}