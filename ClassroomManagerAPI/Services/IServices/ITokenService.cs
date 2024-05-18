using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ClassroomManagerAPI.Services.IServices
{
	public interface ITokenService
	{
		public string generateToken(dynamic authClaims);

		public string decodeToken(String token);
	}
}
