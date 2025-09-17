using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductProductFeature : ShamsiAuditableEntity
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int ProductFeatureId { get; set; }
        public ProductFeature? ProductFeature { get; set; }
    }
}
