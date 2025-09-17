using PriceList.Api.Dtos;
using PriceList.Core.Entities;
using System.Linq.Expressions;

namespace PriceList.Api.Mappings
{
    public class HeaderProductsMapping
    {
        public static readonly Expression<Func<productHeader, productHeadersDto>> ToListItem =
            p => new productHeadersDto(p.Id, p.Key);

        public static productHeadersDto ToListItemDto(productHeader ph)
             => new productHeadersDto(ph.Id, ph.Key);
    }
}
