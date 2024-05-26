using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;

namespace ClassroomManagerAPI.Repositories
{
	public class ReportRepository : BaseRepository<Report>, IReportRepository
	{
		private readonly AppDbContext dbContext;

		public ReportRepository(AppDbContext dbContext) : base(dbContext) 
        {
			this.dbContext = dbContext;
		}
    }
}
