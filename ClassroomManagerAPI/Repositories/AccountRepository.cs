using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;

namespace ClassroomManagerAPI.Repositories
{
    public class AccountRepository : BaseRepository<Account> , IAccountRepository
    {
        public AccountRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}