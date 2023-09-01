using System.Linq.Expressions;

namespace ContactApi.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetById(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}
