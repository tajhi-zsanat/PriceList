using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public static class ProductMappings
    {
        // ✅ Projection expressions (usable inside IQueryable -> Select)
        public static readonly Expression<Func<Product, ProductListItemDto>> ToListItem =
            p => new ProductListItemDto(p.Id, p.Model, p.Price, p.CustomProperties
             .Select(cp => new ProductCustomPropertyItemDto(cp.Key, cp.Value))
             .ToList());

        public static readonly Expression<Func<Product, ProductDetailDto>> ToDetail =
            p => new ProductDetailDto(p.Id, p.Model, p.Model, p.Description, p.Price, p.CreateDateAndTime);

        // ✅ Entity mappers (for create/update)
        public static Product ToEntity(this ProductCreateDto dto) => new()
        {
            Model = dto.Model,
            Description = dto.Description,
            Price = dto.UnitPrice
        };

        public static void Apply(this Product entity, ProductUpdateDto dto)
        {
            entity.Model = dto.Model;
            entity.Description = dto.Description;
            entity.Price = dto.UnitPrice;
        }
    }
}
