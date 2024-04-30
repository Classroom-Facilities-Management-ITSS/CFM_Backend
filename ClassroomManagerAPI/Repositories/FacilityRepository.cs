using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Repositories
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
	{
		private readonly AppDbContext dbContext;
        public FacilityRepository(AppDbContext dbContext) : base(dbContext) { 
			this.dbContext = dbContext;
		}

		public async Task<Facility?> GetByNameAsync(string name)
		{
			return await dbContext.Facilities.FirstAsync(f => f.Name == name);
		}
	}
}
