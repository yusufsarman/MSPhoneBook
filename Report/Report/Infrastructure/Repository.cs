using ReportApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReportApi.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
           
            return await _dbContext.Set<T>().IncludeAndWhereId(id, includes).FirstOrDefaultAsync();

        }
       

        public async Task<T> Add(T entity)
        {
           var data = await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return data.Entity;
        }

        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var data = await _dbContext.Set<T>().FindAsync(id);
            if (data != null)
            {
                _dbContext.Set<T>().Remove(data);
                await _dbContext.SaveChangesAsync();
            }
           
        }
        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}
