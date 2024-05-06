using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClassroomManagerAPI.Repositories
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
	{
		private readonly AppDbContext dbContext;
        public FacilityRepository(AppDbContext dbContext) : base(dbContext) { 
			this.dbContext = dbContext;
		}

        public async Task<IEnumerable<Facility>> GetByNameAsync(string name, int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            int skip = (int)(limit * (page - 1));
            try
            {
                return await dbContext.Facilities.Where(c => c.Name == name && !c.IsDeleted).Skip(skip).Take((int)limit).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex) { throw; }
        }
    }
}
