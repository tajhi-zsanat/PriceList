using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IProductGroupRepository : IGenericRepository<ProductGroup>
    {
        Task<List<ProductGroup>> GetByCategoryIdAsync(int categoryId, CancellationToken ct = default);

        // generic projection to DTO
        Task<List<TResult>> GetByCategoryIdAsync<TResult>(
            int categoryId,
            Expression<Func<ProductGroup, TResult>> selector,
            Func<IQueryable<ProductGroup>, IOrderedQueryable<ProductGroup>>? orderBy = null,
            CancellationToken ct = default);
    }
}
