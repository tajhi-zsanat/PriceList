using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Common;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly ILogger<T> _logger;

        public GenericRepository(AppDbContext db, ILogger<T> logger)
        {
            _db = db;
            _logger = logger;
        }
        protected DbSet<T> Set => _db.Set<T>();

        public IQueryable<T> Query() => Set.AsNoTracking();

        public Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
            => Set.FindAsync([id], ct).AsTask();

        public async Task<TResult?> GetByIdAsync<TResult>(
            int id,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            IQueryable<T> q = Set.AsNoTracking();

            if (predicate is not null)
                q = q.Where(predicate);

            q = q.Where(e => EF.Property<int>(e, "Id") == id);

            return await q.Select(selector).FirstOrDefaultAsync(ct);
        }


        public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
              bool asNoTracking = true,
              CancellationToken ct = default,
              params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = Set;
            foreach (var inc in includes) q = q.Include(inc);
            if (predicate is not null) q = q.Where(predicate);
            if (asNoTracking) q = q.AsNoTracking();
            return await q.ToListAsync(ct);
        }

        // ✅ Server-side projection
        public async Task<List<TResult>> ListAsync<TResult>(Expression<Func<T, bool>>? predicate,
                                                            Expression<Func<T, TResult>> selector,
                                                            bool asNoTracking = true,
                                                            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                            CancellationToken ct = default)
        {
            IQueryable<T> q = Set;

            if (predicate is not null)
                q = q.Where(predicate);

            if (orderBy is not null)
                q = orderBy(q);

            if (asNoTracking)
                q = q.AsNoTracking();

            return await q.Select(selector).ToListAsync(ct);
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<T, bool>> predicate,
                                                                 Expression<Func<T, TResult>> selector,
                                                                 bool asNoTracking = true,
                                                                 CancellationToken ct = default)
        {
            IQueryable<T> q = Set;

            if (asNoTracking) q = q.AsNoTracking();
            if (predicate is not null) q = q.Where(predicate);

            return await q.Select(selector).FirstOrDefaultAsync(ct);
        }

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        { await Set.AddAsync(entity, ct); return entity; }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
            => Set.AddRangeAsync(entities, ct);

        public void RemoveRange(IEnumerable<T> entities)
                => Set.RemoveRange(entities);

        public void Update(T entity) => Set.Update(entity);
        public void Remove(T entity) => Set.Remove(entity);

        public async Task<int> ExecuteDeleteAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
        {
            return await _db.Set<T>()
                .Where(predicate)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => Set.AsNoTracking().AnyAsync(predicate, ct);

        public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
            => predicate is null ? Set.CountAsync(ct) : Set.CountAsync(predicate, ct);

        public async Task<PaginatedResult<T>> ListPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<T> q = Set;

            // includes (only useful when returning entities)
            foreach (var inc in includes) q = q.Include(inc);

            // filter
            if (predicate is not null) q = q.Where(predicate);

            // total before paging
            var total = await q.CountAsync(ct);

            // sorting (recommended to pass a deterministic orderBy)
            if (orderBy is not null) q = orderBy(q);

            // page
            var items = await q.AsNoTracking()
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(ct);

            return new PaginatedResult<T>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        // -----------------------
        // ✅ Paged (projection/DTO)
        // -----------------------
        public async Task<PaginatedResult<TResult>> ListPagedAsync<TResult>(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default,
            params Expression<Func<T, object>>[] includes)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            IQueryable<T> q = Set;

            // includes allow using navs inside the selector
            foreach (var inc in includes) q = q.Include(inc);

            if (predicate is not null) q = q.Where(predicate);

            var total = await q.CountAsync(ct);

            if (orderBy is not null) q = orderBy(q);

            // Compose projection with paging (server-side)
            var items = await q.AsNoTracking()
                               .Select(selector)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(ct);

            return new PaginatedResult<TResult>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
