using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Core.Application.Mappings
{
    public static class ProductMappings
    {
        // ✅ Projection expressions (usable inside IQueryable -> Select)
        public static readonly Expression<Func<Product, ProductListItemDto>> ToListItem =
                p => new ProductListItemDto(
                p.Id,
                p.Description,
                p.DocumentPath,
                p.Price,
                p.Number,
                p.Images.Select(ip => ip.ImagePath).ToList(),
                p.ProductHeaders.Select(cp => new ProductHeaderItemDto(cp.productHeaderId, cp.Value)).ToList()
                //p.productHeaders.Select(ph => new productHeadersDto(ph.Id, ph.Key)).ToList()
    );

        public static ProductListItemDto ToMini(Product p) =>
        new ProductListItemDto(
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
