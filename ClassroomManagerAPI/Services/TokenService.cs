using ClassroomManagerAPI.Services.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ClassroomManagerAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string decodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var result = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (result != null)
            {
                return result.Claims.First(claim => claim.Type.Contains("email")).Value;
            }
            return string.Empty;
        }

        public string generateToken(dynamic authClaims)
        {
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
