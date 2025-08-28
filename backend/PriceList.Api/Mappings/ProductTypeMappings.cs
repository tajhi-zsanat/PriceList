using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public class ProductTypeMappings
    {
        public static readonly Expression<Func<ProductType, ProductTypeListItemDto>> ToListItem =
           p => new ProductTypeListItemDto(p.Id, p.Name, p.ImagePath);

        public static ProductTypeListItemDto ToListItemDto(ProductType g)
             => new ProductTypeListItemDto(g.Id, g.Name, g.ImagePath);
    }
}
