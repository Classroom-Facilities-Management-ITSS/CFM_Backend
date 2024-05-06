using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ClassroomManagerAPI.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;

        public BaseRepository(DbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = _context.Set<T>();
        }
        public virtual async Task<T?> AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity, "newEntity");
            try
            {
                var newEnitity = await _set.AddAsync(entity).ConfigureAwait(continueOnCapturedContext : false);
                await _context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                return newEnitity.Entity;
            }catch (Exception ex) { throw; }
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id, "deleteEntity");
            try
            {
                var entity = await _set.SingleOrDefaultAsync((T c) => c.Id  == id && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
                if (entity == null) return false;
                else
                {
                    entity.IsDeleted = true;
                    entity.UpdatedAt = DateTime.Now;
                    _set.Update(entity);
                    await _context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                }
                return true;
            }catch (Exception ex) { throw; }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int? page, int? limit)
        {
            page = page <=0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            int skip = (int)(limit * (page - 1));
            try
            {
                return await _set.Where((T c) => !c.IsDeleted).Skip(skip).Take((int) limit).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
            }catch (Exception ex) { throw; }
        }

        public virtual async Task<T?> GetByIDAsync(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id, "findEntity");
            try
            {
                return await _set.SingleOrDefaultAsync((T c) => c.Id == id && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex) { throw; }
        }

        public async Task<PaginationModel> Pagination(int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            PaginationModel pagination = new PaginationModel();
            var total = await _set.Where(c => !c.IsDeleted).CountAsync().ConfigureAwait(continueOnCapturedContext: false);
            pagination.Total = (int) Math.Ceiling( total / (decimal)limit);
            pagination.Page = (int) page;
            return pagination;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            try
            {
                var e = await _set.SingleOrDefaultAsync((T c) => c.Id == entity.Id && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
                if (e == null) return default;
                else
                {
                    entity.UpdatedAt = DateTime.Now;
                    PropertyInfo[] properties = e.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite && property.Name != "Id")
                        {
                            object value = property.GetValue(entity);
                            property.SetValue(e, value);
                        }
                    }
                    var updatedEntity = _set.Update(e);
                    await _context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                    return updatedEntity.Entity;
                }
            }
            catch (Exception ex) { throw; }
        }
    }
}
