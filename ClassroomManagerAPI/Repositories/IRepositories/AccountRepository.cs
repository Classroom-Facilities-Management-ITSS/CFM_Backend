using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
	public class AccountRepository : BaseRepository<Account>, IAccountRepository
	{
		private readonly AppDbContext dbContext;
        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
