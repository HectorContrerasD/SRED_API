using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace SRED_API.Helpers
{
    public class JWTHelper
    {
        private readonly IConfiguration configuration;
        public JWTHelper(IConfiguration configuration)
        {
            this.configuration = configuration;  
        }
        public string GetToken(string username, string role, string pass)
        {
           JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var issuer = configuration.GetSection("Jwt").GetValue<string>("Issuer");
            var audience = configuration.GetSection("Jwt").GetValue<string>("Audience");
            var secret = configuration.GetSection("Jwt").GetValue<string>("Secret");
            List<Claim> basicas = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role,role),
                new Claim("password", pass),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim (JwtRegisteredClaimNames.Aud, audience)
            };

            JwtSecurityToken jwt = new(
                issuer,
                audience,
                basicas,
                DateTime.Now,
                DateTime.Now.AddMinutes(20),
                 new SigningCredentials
                (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret ?? "")),
                SecurityAlgorithms.HmacSha256)
                );

            return handler.WriteToken(jwt);
        }
    }
}
