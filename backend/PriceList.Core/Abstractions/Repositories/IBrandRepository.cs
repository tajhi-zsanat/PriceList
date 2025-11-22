using PriceList.Core.Application.Dtos.Brand;
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
        Task<List<BrandListItemDto>> GetBrandsAsync(int categoryId, int groupId, string? search, CancellationToken ct = default);
    }
}
