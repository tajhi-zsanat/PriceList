using PriceList.Api.Dtos;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class CategoryMappings
    {
        public static readonly Expression<Func<Category, CategoryListItemDto>> ToListItem =
        p => new CategoryListItemDto(p.Id, p.Name, p.ImagePath);

        public static CategoryListItemDto ToListItemDto(Category p)
            => new CategoryListItemDto(p.Id, p.Name, p.ImagePath);
    }
}
