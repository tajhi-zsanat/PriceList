using PriceList.Api.Dtos;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductGroupApiMappers
    {
        // Use after materialization (e.g., over List<ProductGroup>)
        public static ProductGroupListItemDto ToListItemDto(ProductGroup g)
            => new(g.Id, g.Name, g.ImagePath);
    }
}
