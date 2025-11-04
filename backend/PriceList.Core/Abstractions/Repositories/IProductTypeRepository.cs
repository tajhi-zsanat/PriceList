using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IProductTypeRepository : IGenericRepository<ProductType>
    {
        Task<List<ProductType>> GetByGroupIdAsync(int ProductGroupId, CancellationToken ct = default);

        // generic projection to DTO
        Task<List<TResult>> GetByGroupIdAsync<TResult>(
            int categoryId,
            Expression<Func<ProductType, TResult>> selector,
            Func<IQueryable<ProductType>, IOrderedQueryable<ProductType>>? orderBy = null,
            CancellationToken ct = default);

        Task AddFormTypeAsync(int formId,
            int typeId,
            int displayOrder,
            string? color,
            CancellationToken ct = default);
    }
}
