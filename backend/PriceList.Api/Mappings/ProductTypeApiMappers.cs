using PriceList.Api.Dtos;
using PriceList.Core.Application.Dtos.ProductType;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductTypeApiMappers
    {
        // Use only after materializing entities (e.g., List<ProductType>)
        public static ProductTypeListItemDto ToListItemDto(ProductType p) =>
            new(
                p.Id,
                p.Name,
                p.ImagePath,
            );
    }
}
