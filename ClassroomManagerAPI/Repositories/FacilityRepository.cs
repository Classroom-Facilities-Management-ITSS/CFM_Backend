using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClassroomManagerAPI.Repositories
{
    public class FacilityRepository : BaseRepository<Facility>, IFacilityRepository
	{
		private readonly AppDbContext _dbContext;
        public FacilityRepository(AppDbContext dbContext) : base(dbContext) { 
			_dbContext = dbContext;
		}

		public async Task<int> CountFacilitiesByStatusAsync(Guid classroomId, FacilityStatusEnum status)
		{
			try
			{
				return await _dbContext.Facilities.Where(c => c.ClassroomId == classroomId && c.Status == status).CountAsync();
			}
			catch (Exception ex) { throw; }
		}

		public async Task<int> CountFacilitiesByStatusAsync(string ClassroomAddress, FacilityStatusEnum status)
		{
			try
			{
				return await _dbContext.Facilities.Where(c => c.Classroom.Address == ClassroomAddress && c.Status == status).CountAsync();
			}
			catch (Exception ex) { throw; }
		}

		public async Task<IEnumerable<Facility>> GetByNameAsync(string name, int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            int skip = (int)(limit * (page - 1));
            try
            {
                return await _dbContext.Facilities.Where(c => c.Name == name && !c.IsDeleted).Skip(skip).Take((int)limit).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex) { throw; }
        }
    }
}
