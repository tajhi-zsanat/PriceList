using PriceList.Core.Application.Dtos.Brand;
using PriceList.Core.Application.Dtos.Feature;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Mappings
{
    public class FeatureMappings
    {
        public static readonly Expression<Func<FormFeature, FeatureData>> item =
         b => new FeatureData(b.Id,
             b.Name,
             b.DisplayOrder,
             b.Rows.Select(r => r.Id).ToList()
          );
    }
}
