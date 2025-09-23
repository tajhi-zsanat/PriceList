using PriceList.Core.Application.Dtos.ProductType;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public static class ProductTypeMappings
    {
        // ✅ EF-translatable; use inside IQueryable.Select(...)
        public static readonly Expression<Func<ProductType, ProductTypeListItemDto>> ToListItem =
            p => new ProductTypeListItemDto(
                p.Id,
                p.Name,
                p.ImagePath,
                p.ProductTypeFeatures
                    .OrderBy(tf => tf.Feature.Name)
                    .Select(tf => new ProductFeatureItemDto(tf.Feature.Id, tf.Feature.Name))
                    .ToList()
            );
    }
}
