using System.Linq.Expressions;

namespace ReportApi.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> GetById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);
        Task<T> Add(T entity, CancellationToken cancellationToken = default);
        Task AddRange(IEnumerable<T> entity, CancellationToken cancellationToken = default);
        Task Update(T entity, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}
