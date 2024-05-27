using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Repositories
{
    public class AccountRepository : BaseRepository<Account> , IAccountRepository
    {
		private readonly AppDbContext _dbContext;

		public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
		}

		public async Task<Account?> GetByEmailAsync(string email)
		{
			try
			{
				return await _dbContext.Accounts.Where(c => c.Email == email && !c.IsDeleted).FirstOrDefaultAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}