using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class ProductGroupRepository : GenericRepository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(AppDbContext db, ILogger<ProductGroup> logger)
        : base(db, logger)
        {
        }

        public Task<List<ProductGroup>> GetByCategoryIdAsync(int categoryId, CancellationToken ct = default)
        => Set.Where(pg => pg.CategoryId == categoryId)
              .AsNoTracking()
              .ToListAsync(ct);

        public Task<List<TResult>> GetByCategoryIdAsync<TResult>(
             int categoryId,
             Expression<Func<ProductGroup, TResult>> selector,
             Func<IQueryable<ProductGroup>, IOrderedQueryable<ProductGroup>>? orderBy = null,
             CancellationToken ct = default)
        {
            IQueryable<ProductGroup> q = Set.Where(pg => pg.CategoryId == categoryId);

            if (orderBy is not null)
                q = orderBy(q);

            return q.AsNoTracking()
                    .Select(selector)
                    .ToListAsync(ct);
        }
    }
}
