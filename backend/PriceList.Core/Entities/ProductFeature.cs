using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductFeature : ShamsiAuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int FeatureId { get; set; }
        public Feature Feature { get; set; } = null!;
    }
}
