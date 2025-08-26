using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public class BrandMappings
    {
        public static readonly Expression<Func<Brand, BrandListItemDto>> ToListItem =
            p => new BrandListItemDto(p.Id, p.Name, p.ImagePath);
    }
}
