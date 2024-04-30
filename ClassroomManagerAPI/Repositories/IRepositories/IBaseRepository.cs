﻿using ClassroomManagerAPI.Entities;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIDAsync(Guid id);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(int? page, int? limit);

    }
}