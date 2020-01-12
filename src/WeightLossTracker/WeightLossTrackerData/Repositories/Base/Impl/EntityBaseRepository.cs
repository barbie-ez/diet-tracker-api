using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeightLossTrackerData.DataContext;
using WeightLossTrackerData.Helpers;
using WeightLossTrackerData.Repositories.Base.Interface;

namespace WeightLossTrackerData.Repositories.Base.Impl
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        protected ApplicationDbContext _context;

        public EntityBaseRepository(ApplicationDbContext context)
        {
            _context = context;     
        }

        public async Task<T> AddAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);


            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                EntityEntry dbEntityEntry = _context.Entry<T>(item);
                _context.Set<T>().Add(item);
            }
            await _context.SaveChangesAsync();
        }



        public async Task<int> AddReturnAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }


        public async Task<int> AddBoolReturnAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(T entity)
        {
            EntityEntry dbEntityEntny = _context.Entry<T>(entity);
            dbEntityEntny.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                EntityEntry dbEntityEntny = _context.Entry<T>(item);
                dbEntityEntny.State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> AllIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return await query.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
            return await query.Where(predicate).ToListAsync();
        }

        public async virtual Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }
        public Task<int> CountAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(T entity)
        {
            EntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = _context.Set<T>().Where(predicate);
            foreach (var entity in entities)
            {
                _context.Entry<T>(entity).State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
                query = query.Include(property);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
                query = query.Include(property);
            return await query.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<PagedList<T>> GetAllAsync(ResourceParameters resourceParameters)
        {
            var collectionBeforePaging = _context.Set<T>();

            return PagedList<T>.Create(collectionBeforePaging, resourceParameters.PageNumber, resourceParameters.PageSize);
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }
        public IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
                query = query.Include(property);
            return query.Where(predicate).AsQueryable();
        }
        public IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
                query = query.Include(property);
            return query.AsQueryable();
        }
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetLastAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).LastOrDefaultAsync();
        }
        public async Task<T> GetLastAsync()
        {
            return await _context.Set<T>().LastOrDefaultAsync();
        }
        public async Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            return await query.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<T> GetSingleAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var property in includeProperties)
                query = query.Include(property);
            return await query.Where(predicate).FirstOrDefaultAsync();
        }


    }
}
