using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ContactApi.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeAndWhereId<T>(
            this IQueryable<T> queryable,
            Guid id,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }

            var idProperty = typeof(T).GetProperty("Id");
            var parameter = Expression.Parameter(typeof(T), "e");
            var idExpression = Expression.Property(parameter, idProperty);
            var equalsExpression = Expression.Equal(idExpression, Expression.Constant(id));

            var whereExpression = Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);
            return queryable.Where(whereExpression);
        }
        
    }
}
