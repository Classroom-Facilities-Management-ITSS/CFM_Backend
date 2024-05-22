using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IAuthRepository : IBaseRepository<Account>
    {
        public Task<Guid> Register(Account user);
        
        public Task<Account> LogIn(Account user);
		public Task<bool> Active(string email);
        public Task<Account> GetByEmailAsync(string email);		
        public Task<bool> GenerateNewPassword(string email); 

	}
}
