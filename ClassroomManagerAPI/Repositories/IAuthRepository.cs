using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;

namespace ClassroomManagerAPI.Repositories
{
	public interface IAuthRepository
	{
		public Task<bool> Register(AddUserRequestDto user);
		public Task<string> LogIn(AddUserRequestDto user);
		public Task<bool> Active(String token);
		public Task<bool> UpdatePassword(UpdatePasswordRequestDto user);
		public Task<bool> GenerateNewPassword(String email);
		public Task<List<Account>> GetAllAccountsAsync();
		public Task<Account> GetAccountByEmailAsync(String email);
		public Task<Account> GetAccountByIdAsync(Guid id);
		public Task<Account?> UpdateAsync(Guid id, Account account);
		public Task<Account> DeleteAsync(Guid id);
	}
}
