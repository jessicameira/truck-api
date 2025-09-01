using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace TruckControl.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("generate-token")]
        public IActionResult GenerateToken()
        {
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? "FallbackSuperSecretKey123!";
                var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "1440");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("app", "trcuk-control"),
                        new Claim("timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { token = tokenString, expiresIn = expiryInMinutes * 60 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao gerar token", details = ex.Message });
            }
        }
    }
}