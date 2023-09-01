﻿using System.Linq.Expressions;

namespace ReportApi.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int id, params Expression<Func<T, object>>[] includes);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}