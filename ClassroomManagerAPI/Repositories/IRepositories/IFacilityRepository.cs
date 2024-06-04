using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IFacilityRepository : IBaseRepository<Facility?>
    {
        public Task<IEnumerable<Facility>> GetByNameAsync(string name, int? page, int? limit);
        public Task<int> CountFacilitiesByStatusAsync(Guid classroomId, FacilityStatusEnum status);
	}
}
