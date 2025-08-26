using Microsoft.EntityFrameworkCore;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class BrandRepository(AppDbContext db) : GenericRepository<Brand>(db), IBrandRepository
    {
        public Task<Brand?> GetByNameAsync(string name, CancellationToken ct = default)
            => Set.AsNoTracking()
                  .FirstOrDefaultAsync(b => b.Name == name, ct);

        public Task<List<Brand>> ByProductTypeAsync(int typeId, CancellationToken ct = default)
            => Set.Where(b => b.ProductTypes.Any(pt => pt.Id == typeId))
                  .OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
                  .AsNoTracking()
                  .ToListAsync(ct);

        public Task<List<TResult>> ByProductTypeAsync<TResult>(
            int typeId,
            Expression<Func<Brand, TResult>> selector,
            Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null,
            CancellationToken ct = default)
        {
            IQueryable<Brand> q = Set.Where(b => b.ProductTypes.Any(pt => pt.Id == typeId));

            if (orderBy is not null)
                q = orderBy(q);

            return q.AsNoTracking()
                    .Select(selector)
                    .ToListAsync(ct);
        }
    }
}
