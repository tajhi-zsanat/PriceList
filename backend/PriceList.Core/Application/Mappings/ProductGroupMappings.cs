using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class ProductGroupMappings
    {
        // ✅ EF-translatable projections (use inside IQueryable.Select)
        public static readonly Expression<Func<ProductGroup, ProductGroupListItemDto>> ToListItem =
            p => new ProductGroupListItemDto(p.Id, p.Name, p.ImagePath);

        public static readonly Expression<Func<ProductGroup, ProductGroupDetailDto>> ToDetail =
            g => new ProductGroupDetailDto(g.Id, g.Name, g.ImagePath, g.CategoryId, g.DisplayOrder);
    }
}
