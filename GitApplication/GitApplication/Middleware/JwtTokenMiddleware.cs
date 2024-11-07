

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GitApplication.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;

        public JwtTokenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = GetTokenFromRequest(context);

            if (!string.IsNullOrEmpty(token) && ValidateToken(token, out var username))
            {
                context.Items["Username"] = username;  // מכניסים את שם המשתמש בהקשר של הבקשה
            }

            await _next(context);
        }

        private string GetTokenFromRequest(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues headerValue))
            {
                var token = headerValue.ToString().Replace("Bearer ", "");
                return token;
            }
            return string.Empty;
        }

        private bool ValidateToken(string token, out string username)
        {
            username = string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "MyApp",
                    ValidAudience = "MyAppUsers",
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                username = jwtToken?.Claims.FirstOrDefault(c => c.Type == "username")?.Value; // חיפוש שם המשתמש בטוקן

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

