using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core6_EShop.Cls
{
    public class Helper
    {
        public Helper()
        { }
        public string CreateToken(IConfiguration configuration, string username, double addMinutes = 30)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", Convert.ToString(username)),
                }),
                Expires = DateTime.UtcNow.AddMinutes(addMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
