using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Entities;

namespace PriceList.Api.Mappings
{
    public static class ProductHeaderApiMappers
    {
        public static HeaderListItemDto ToListItemDto(Header h)
            => new(h.Id, h.Key); // or h.Name
    }
}
