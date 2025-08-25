using Microsoft.EntityFrameworkCore;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class GenericRepository<T>(AppDbContext db) : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext Db = db;
        protected DbSet<T> Set => Db.Set<T>();

        public Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => Set.FindAsync([id], ct).AsTask();

        public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
                                             CancellationToken ct = default,
                                             params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = Set;
            foreach (var inc in includes) q = q.Include(inc);
            if (predicate is not null) q = q.Where(predicate);
            return await q.AsNoTracking().ToListAsync(ct);
        }

        // ✅ Server-side projection
        public async Task<List<TResult>> ListAsync<TResult>(Expression<Func<T, bool>>? predicate,
                                                            Expression<Func<T, TResult>> selector,
                                                            CancellationToken ct = default)
        {
            IQueryable<T> q = Set;
            if (predicate is not null) q = q.Where(predicate);
            return await q.AsNoTracking().Select(selector).ToListAsync(ct);
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> predicate,
                                                                 Expression<Func<T, TResult>> selector,
                                                                 CancellationToken ct = default)
            => await Set.AsNoTracking().Where(predicate).Select(selector).FirstOrDefaultAsync(ct);

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        { await Set.AddAsync(entity, ct); return entity; }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
            => Set.AddRangeAsync(entities, ct);

        public void Update(T entity) => Set.Update(entity);
        public void Remove(T entity) => Set.Remove(entity);

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => Set.AsNoTracking().AnyAsync(predicate, ct);

        public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null ? Set.CountAsync(ct) : Set.CountAsync(predicate, ct);
    }
}
