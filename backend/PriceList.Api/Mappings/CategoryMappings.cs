using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class CategoryMappings
    {// ✅ Projection expressions (usable inside IQueryable -> Select)
        public static readonly Expression<Func<Category, CategoryListItemDto>> ToListItem =
            p => new CategoryListItemDto(p.Id, p.Name, p.ImagePath);
    }
}
