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
    public class ProductTypeRepository(AppDbContext db) : GenericRepository<ProductType>(db), IProductTypeRepository
    {
        public Task<List<ProductType>> GetByGroupIdAsync(int ProductGroupId, CancellationToken ct = default)
        => Set.Where(pt => pt.ProductGroupId == ProductGroupId)
              .AsNoTracking()
              .ToListAsync(ct);

        public Task<List<TResult>> GetByGroupIdAsync<TResult>(
             int productGroupId,
             Expression<Func<ProductType, TResult>> selector,
             Func<IQueryable<ProductType>, IOrderedQueryable<ProductType>>? orderBy = null,
             CancellationToken ct = default)
        {
            IQueryable<ProductType> q = Set.Where(pt => pt.ProductGroupId == productGroupId);

            if (orderBy is not null)
                q = orderBy(q);

            return q.AsNoTracking()
                    .Select(selector)
                    .ToListAsync(ct);
        }
    }
}
