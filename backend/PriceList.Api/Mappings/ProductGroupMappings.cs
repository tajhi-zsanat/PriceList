using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductGroupMappings
    {
        public static readonly Expression<Func<ProductGroup, ProductGroupListItemDto>> ToListItem =
            p => new ProductGroupListItemDto(p.Id, p.Name, p.ImagePath);
    }
}
