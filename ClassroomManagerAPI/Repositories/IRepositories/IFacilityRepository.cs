using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IFacilityRepository : IBaseRepository<Facility?>
    {
        public Task<IEnumerable<Facility>> GetByNameAsync(string name, int? page, int? limit);
    }
}
