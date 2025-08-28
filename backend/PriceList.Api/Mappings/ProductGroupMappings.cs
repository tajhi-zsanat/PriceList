using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductGroupMappings
    {
        public static readonly Expression<Func<ProductGroup, ProductGroupListItemDto>> ToListItem =
            p => new ProductGroupListItemDto(p.Id, p.Name, p.ImagePath);

        public static readonly Expression<Func<ProductGroup, ProductGroupDetailDto>> ToDetail =
            g => new ProductGroupDetailDto(g.Id, g.Name, g.ImagePath, g.CategoryId, g.DisplayOrder);

        public static ProductGroupListItemDto ToListItemDto(ProductGroup g)
            => new ProductGroupListItemDto(g.Id, g.Name, g.ImagePath);
    }
}
