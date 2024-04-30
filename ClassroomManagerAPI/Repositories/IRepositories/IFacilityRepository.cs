using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IFacilityRepository : IBaseRepository<Facility?>
    {
        public Task<Facility?> GetByNameAsync(string name);
    }
}
