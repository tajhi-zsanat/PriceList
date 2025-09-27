using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Entities;

namespace PriceList.Api.Mappings
{
    public static class ProductApiMappers
    {
        // Imperative mapper (use AFTER materialization, e.g., in controllers)
        public static ProductListItemDto ToListItemDto(Product p) =>
            new(
                p.Id,
                p.Description,
                p.DocumentPath,
                p.Price,
                p.Number,
                p.Images?.Select(i => i.ImagePath).ToList() ?? new List<string>(),
                p.ProductHeaders?
                    .Select(cp => new ProductHeaderItemDto(cp.productHeaderId, cp.Value))
                    .ToList() ?? new List<ProductHeaderItemDto>()
            );
    }
}
