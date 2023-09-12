using ReportApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReportApi.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContextFactory _dbContextFactory;
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
        }

        public virtual async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetById(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {

            return await _dbContext.Set<T>().IncludeAndWhereId(id, includes).FirstOrDefaultAsync(cancellationToken);

        }


        public virtual async Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            var data = await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return data.Entity;
        }
        public virtual async Task AddRange(IEnumerable<T> entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public virtual async Task Update(T entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var data = await _dbContext.Set<T>().FindAsync(id);
            if (data != null)
            {
                _dbContext.Set<T>().Remove(data);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

        }
        public virtual IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }


    }
}
