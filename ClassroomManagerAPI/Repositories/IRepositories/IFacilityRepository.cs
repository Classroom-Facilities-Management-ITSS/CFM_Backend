using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IFacilityRepository
    {
        Task<List<Facility>> GetAllAsync();
        Task<Facility?> GetByIdAsync(Guid id);
        Task<Facility?> GetByNameAsync(string name);
        Task<Facility> CreateAsync(Facility facility);
        Task<Facility?> UpdateAsync(Guid id, Facility facility);
        Task<Facility> DeleteAsync(Guid id);
    }
}
