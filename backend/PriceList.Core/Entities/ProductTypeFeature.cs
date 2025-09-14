using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductTypeFeature : ShamsiAuditableEntity
    {
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public int ProductFeatureId { get; set; }
        public ProductFeature ProductFeature { get; set; } = null!;
    }
}
