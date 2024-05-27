using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Repositories
{
	public class ReportRepository : BaseRepository<Report>, IReportRepository
	{
		private readonly AppDbContext _dbContext;

		public ReportRepository(AppDbContext dbContext) : base(dbContext) 
        {
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Report>> GetByClassroomAddressAsync(string address, int? page, int? limit)
		{
			page = page <= 0 ? 1 : page ?? 1;
			limit = limit <= 0 ? 10 : limit ?? 10;
			int skip = (int)(limit * (page - 1));
			try
			{
				var classrooms = await _dbContext.Classrooms
					.Include(x => x.Facilities)
					.Include(x => x.Reports)
					.Where(c => c.Address == address && !c.IsDeleted)
					.Skip(skip).Take((int)limit)
					.ToListAsync()
					.ConfigureAwait(false);
				// the line below is not the correct code, need to rewrite
				return classrooms.SelectMany(c => c.Reports).ToList();
			} 
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
