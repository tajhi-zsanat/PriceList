using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand?> GetByNameAsync(string name, CancellationToken ct = default);

        // Brands filtered by ProductTypeId (M2M)
        Task<List<Brand>> ByProductTypeAsync(int typeId, CancellationToken ct = default);

        // Projection overload to avoid over-fetching
        Task<List<TResult>> ByProductTypeAsync<TResult>(
            int typeId,
            Expression<Func<Brand, TResult>> selector,
            Func<IQueryable<Brand>, IOrderedQueryable<Brand>>? orderBy = null,
            CancellationToken ct = default);
    }
}
