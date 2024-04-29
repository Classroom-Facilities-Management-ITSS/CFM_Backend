using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagerAPI.Repositories
{
    public class SQLFacilityRepository : IFacilityRepository
	{
		private readonly AppDbContext dbContext;
        public SQLFacilityRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Facility> CreateAsync(Facility facility)
		{
			await dbContext.Facilities.AddAsync(facility);
			await dbContext.SaveChangesAsync();
			return facility;
		}

		public async Task<Facility> DeleteAsync(Guid id)
		{
			var existingFacility = await dbContext.Facilities.FirstOrDefaultAsync(f => f.Id == id);

			if (existingFacility == null)
			{
				return null;
			}

			dbContext.Facilities.Remove(existingFacility);
			await dbContext.SaveChangesAsync();
			return existingFacility;
		}

		public async Task<List<Facility>> GetAllAsync()
		{
			return await dbContext.Facilities.ToListAsync();
		}

		public async Task<Facility?> GetByIdAsync(Guid id)
		{
			return await dbContext.Facilities.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<Facility?> GetByNameAsync(string name)
		{
			return await dbContext.Facilities.FirstAsync(f => f.Name == name);
		}

		public async Task<Facility?> UpdateAsync(Guid id, Facility facility)
		{
			var existingFacility = await dbContext.Facilities.FirstOrDefaultAsync(r => r.Id == id);

			if (existingFacility == null)
			{
				return null;
			}

			existingFacility.Name = facility.Name;
			existingFacility.Count = facility.Count;
			existingFacility.Status = facility.Status;
			existingFacility.Version = facility.Version;
			existingFacility.Note = facility.Note;

			await dbContext.SaveChangesAsync();
			return existingFacility;
		}
	}
}
