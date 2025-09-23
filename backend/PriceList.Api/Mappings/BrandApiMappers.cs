using PriceList.Api.Dtos;
using PriceList.Core.Application.Dtos.Brand;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class BrandApiMappers
    {
        // Use only after entities are materialized (e.g., List<Brand>)
        public static BrandListItemDto ToListItemDto(Brand b)
            => new(b.Id, b.Name, b.ImagePath);
    }
}
