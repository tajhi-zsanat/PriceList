using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
                                CancellationToken ct = default,
                                params Expression<Func<T, object>>[] includes);

        // ✅ New: project to a DTO/result type on the DB side
        Task<List<TResult>> ListAsync<TResult>(Expression<Func<T, bool>>? predicate,
                                               Expression<Func<T, TResult>> selector,
                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                               CancellationToken ct = default);


        Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> predicate,
                                                    Expression<Func<T, TResult>> selector,
                                                    CancellationToken ct = default);

        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
        void Update(T entity);
        void Remove(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);

        Task<PaginatedResult<T>> ListPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes);

        Task<PaginatedResult<TResult>> ListPagedAsync<TResult>(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes);
    }
}
