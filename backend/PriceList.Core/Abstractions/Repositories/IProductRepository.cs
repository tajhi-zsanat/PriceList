using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> SearchAsync(string? term, CancellationToken ct = default);

        Task<int> GetTopSupplierAsync(int categoryId, int groupId, int typeId, int brandId, CancellationToken ct = default);
    }
}
