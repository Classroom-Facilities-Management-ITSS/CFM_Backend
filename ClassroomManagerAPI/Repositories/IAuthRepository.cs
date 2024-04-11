using ClassroomManagerAPI.Models.Dto;

namespace ClassroomManagerAPI.Repositories
{
	public interface IAuthRepository
	{
		public Task<bool> Register(AddUserRequestDto user);
		public Task<string> LogIn(AddUserRequestDto user);
		public Task<bool> Active(String token);
    }
}
