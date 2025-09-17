using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductMappings
    {
        // ✅ Projection expressions (usable inside IQueryable -> Select)
        public static readonly Expression<Func<Product, ProductListItemDto>> ToListItem =
                p => new ProductListItemDto(
                p.Id,
                p.Model,
                p.Description,
                p.DocumentPath,
                p.Price,
                p.Number,
                p.Images.Select(ip => ip.ImagePath).ToList()
                //p.CustomProperties.Select(cp => new ProductCustomPropertyItemDto(cp.Key, cp.Value)).ToList()
    );

        public static ProductListItemDto ToListItemDto(Product p)
        {
            return new ProductListItemDto(
                p.Id,
                p.Model,
                p.Description,
                p.DocumentPath,
                p.Price,
                p.Number,
                p.Images?.Select(i => i.ImagePath).ToList() ?? new List<string>()
                //p.CustomProperties?
                // .Select(cp => new ProductCustomPropertyItemDto(cp.pr, cp.Value))
                // .ToList() ?? new List<ProductCustomPropertyItemDto>()
            );
        }
    }
}
