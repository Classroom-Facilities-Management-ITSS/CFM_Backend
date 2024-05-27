using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClassroomManagerAPI.Repositories
{
	public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository
	{
		private readonly AppDbContext _dbContext;
		public ClassroomRepository(AppDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Classroom>?> GetByAddressAsync(string address, int? page, int? limit)
		{
			page = page <= 0 ? 1 : page ?? 1;
			limit = limit <= 0 ? 10 : limit ?? 10;
			int skip = (int)(limit * (page - 1));
			try
			{
				return await _dbContext.Classrooms.Include(x => x.Facilities).Include(x => x.Reports).Where(c => c.Address == address && !c.IsDeleted).Skip(skip).Take((int)limit).ToListAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
