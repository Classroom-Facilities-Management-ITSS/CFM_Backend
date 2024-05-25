using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
	public interface IAccountRepository : IBaseRepository<Account?>
	{
		public Task<Account?> GetByEmailAsync(string email);		
	}
}
