using PriceList.Core.Application.Dtos.Brand;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class BrandMappings
    {
        // ✅ Use inside IQueryable.Select(...)
        public static readonly Expression<Func<Brand, BrandListItemDto>> ToListItem =
            b => new BrandListItemDto(b.Id, b.Name, b.ImagePath);
    }
}
