using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFeatureRepository : IGenericRepository<Feature>
    {
        Task<ScrollResult<ProductFeatureWithProductsDto>> ListFeaturesWithProductsScrollAsync(
        int skip,
        int take,
        int? categoryId = null,
        int? groupId = null,
        int? typeId = null,
        int? brandId = null,
        int? supplierId = null,
        string? productSearch = null,
        CancellationToken ct = default);
    }
}
