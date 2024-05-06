using ClassroomManagerAPI.Services.IServices;

namespace ClassroomManagerAPI.Services
{
    public class BCryptService : IBCryptService
	{
		public string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		public bool verifyPassword(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}
