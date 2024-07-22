using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendCalpar.Models;

namespace BackendCalpar.Services
{
    public class TokenServices
    {
        public static Object GenerateToken()
        {
            var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e"); 

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, DateTime.Now.ToString())
            };

            var expiraEm = DateTime.UtcNow.AddHours(2);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = expiraEm,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var jwtToken = tokenHandler.WriteToken(token);

            return new TokenResponseModel()
            {
                Token = jwtToken,
            };
        }       
    }
}
