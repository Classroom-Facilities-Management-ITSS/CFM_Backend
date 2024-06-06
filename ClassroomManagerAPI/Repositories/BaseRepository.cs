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

        public virtual IEnumerable<T?> GetPaginationEntity(IEnumerable<T> entity, int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            int skip = (int)(limit * (page - 1));
            try
            {
                return  entity.Where((T c) => !c.IsDeleted).Skip(skip).Take((int) limit).ToList();
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

        public virtual async Task<int> GetCountAsync()
        {
            return await _set.CountAsync().ConfigureAwait(continueOnCapturedContext : false);
        }

        public virtual async Task<StatisticModel> GetStatisticAsync()
        {
            var count = await GetCountAsync().ConfigureAwait(false);
            var now = DateTime.Now.Date;
            var yesterday = DateTime.Now.Date.AddDays(-1);
            var lastMonth = DateTime.Now.Date.AddMonths(-1);
            var createByDay = await _set.Where(c => c.CreatedAt >= yesterday).CountAsync().ConfigureAwait(continueOnCapturedContext: false);
            var createByMonth = await _set.Where(c => c.CreatedAt >= lastMonth).CountAsync().ConfigureAwait(continueOnCapturedContext: false);
            return new StatisticModel
            {
                Count = count,
                NewCreateByDay = createByDay,
                NewCreateByMonth = createByMonth
            };
        }

        public virtual async Task<PaginationModel> Pagination(int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            PaginationModel pagination = new PaginationModel();
            var total = await _set.Where(c => !c.IsDeleted).CountAsync().ConfigureAwait(continueOnCapturedContext: false);
            pagination.Total = (int) Math.Ceiling( total / (decimal)limit);
            pagination.Page = (int) page;
            return pagination;
        }

        public virtual IQueryable<T?> Queryable
        {
            get
            {
                IQueryable<T> queryable = _context.Set<T>().AsQueryable();
                return queryable;
            }   
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            try
            {
                var existingEntity = await _set.SingleOrDefaultAsync(e => e.Id == entity.Id && !e.IsDeleted)
                    .ConfigureAwait(continueOnCapturedContext: false);
                if (existingEntity == null)
                {
                    return default;
                }
                else
                {
                    entity.UpdatedAt = DateTime.Now;
                    PropertyInfo[] properties = entity.GetType().GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.CanRead && property.CanWrite && property.Name != "Id")
                        {
                            object newValue = property.GetValue(entity);
                            object oldValue = property.GetValue(existingEntity);

                            if (!object.Equals(newValue, oldValue))
                            {
                                property.SetValue(existingEntity, newValue);
                            }
                        }
                    }

                    var foreignKeyProperties = _context.Model.FindEntityType(typeof(T)).GetForeignKeys()
                        .SelectMany(fk => fk.Properties)
                        .Select(p => p.Name);

                    foreach (var fkProperty in foreignKeyProperties)
                    {
                        var property = properties.SingleOrDefault(p => p.Name == fkProperty);
                        if (property != null)
                        {
                            object newValue = property.GetValue(entity);
                            object oldValue = property.GetValue(existingEntity);

                            if (!object.Equals(newValue, oldValue))
                            {
                                property.SetValue(existingEntity, newValue);
                            }
                        }
                    }

                    _context.Entry(existingEntity).State = EntityState.Modified;
                    await _context.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false);
                    return existingEntity;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public PaginationModel PaginationEntity(IEnumerable<T> entity, int? page, int? limit)
        {
            page = page <= 0 ? 1 : page ?? 1;
            limit = limit <= 0 ? 10 : limit ?? 10;
            PaginationModel pagination = new PaginationModel();
            var total = entity.Where(x => !x.IsDeleted).Count();
            pagination.Total = (int)Math.Ceiling(total / (decimal)limit);
            pagination.Page = (int)page;
            return pagination;
        }
    }
}
