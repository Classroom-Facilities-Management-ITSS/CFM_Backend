using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;

namespace ClassroomManagerAPI.Repositories
{
	public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
	{
		private readonly AppDbContext _dbContext;

		public ScheduleRepository(AppDbContext dbContext) : base(dbContext)
        {
			_dbContext = dbContext;
		}
	}
}
