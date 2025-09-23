using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class ProductHeaderMappings
    {
        // If your entity property is Key, map Key -> Name
        public static readonly Expression<Func<Header, HeaderListItemDto>> ToListItem =
            h => new HeaderListItemDto(h.Id, h.Key);
    }
}
