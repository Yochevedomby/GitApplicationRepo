using GitApplication.Models;
using GitApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GitApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly IConfiguration _configuration;
        public LoginController(SessionService sessionService, IConfiguration configuration)
        {
            _sessionService = sessionService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var user = UserDB.Authenticate(request.Username, request.Password);
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            // יצירת טוקן JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSettings:SecretKey")); // המפתח הסודי
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   
            new Claim("UserName", user.Username),
            //new Claim("UserPassword", user.Password),
            new Claim("Bookmarks", string.Join(",", user.BookmarkedRepositories)) // שמירת סימניות בטוקן
                }),
                Expires = DateTime.UtcNow.AddHours(1), // טוקן בתוקף לשעה
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString, UserName= user.Username }); // החזרת הטוקן ללקוח
           
        }

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    _sessionService.ClearUserSession();
        //    return Ok("Logout successful");
        //}
    }
}
