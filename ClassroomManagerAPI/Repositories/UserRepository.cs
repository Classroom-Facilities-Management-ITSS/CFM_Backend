using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;

namespace ClassroomManagerAPI.Repositories
{
    public class UserRepository : BaseRepository<UserInfo> , IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
