using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T?> Queryable { get; }
        Task<T?> GetByIDAsync(Guid id);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        IEnumerable<T?> GetPaginationEntity(IEnumerable<T> entity,int? page, int? limit);
        PaginationModel PaginationEntity(IEnumerable<T> entity, int? page, int? limit);
        Task<IEnumerable<T>> GetAllAsync(int? page, int? limit);
        Task<PaginationModel> Pagination(int? page, int? limit);
        Task<int> GetCountAsync();
        Task<StatisticModel> GetStatisticAsync();
    }
}
