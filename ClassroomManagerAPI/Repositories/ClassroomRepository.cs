using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Repositories
{
	public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository
	{
		private readonly AppDbContext dbContext;
		public ClassroomRepository(AppDbContext dbContext) : base(dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<Classroom?> GetByAddressAsync(string address)
		{
			try
			{
				return await dbContext.Classrooms.Where(c => c.Address == address && !c.IsDeleted).FirstOrDefaultAsync().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
