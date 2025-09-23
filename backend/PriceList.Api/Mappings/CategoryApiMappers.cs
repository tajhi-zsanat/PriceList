using PriceList.Api.Dtos;
using PriceList.Core.Application.Dtos.Category;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class CategoryApiMappers
    {
        // Use after materialization (e.g., controller LINQ over List<Category>)
        public static CategoryListItemDto ToListItemDto(Category p)
            => new(p.Id, p.Name, p.ImagePath);
    }
}
