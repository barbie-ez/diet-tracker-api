using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WeightLossTracker.DataStore.Repositories.Base.Interface
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> AllIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetAllAsQueryable();
        IQueryable<T> GetAllAsQueryable(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync();
        Task<int> CountAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetLastAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetLastAsync();
        //  Task<T> GetLast(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(int id);
        Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<int> AddReturnAsync(T entity);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(List<T> entity);
        Task UpdateAsync(T entity);
        Task UpdateAsync(List<T> entity);
        Task DeleteAsync(T entity);
        Task DeleteWhereAsync(Expression<Func<T, bool>> predicate);
        Task CommitAsync();
        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
    }

}
