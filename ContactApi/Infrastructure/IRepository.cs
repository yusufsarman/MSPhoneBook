using System.Linq.Expressions;

namespace ContactApi.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int id);        
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
