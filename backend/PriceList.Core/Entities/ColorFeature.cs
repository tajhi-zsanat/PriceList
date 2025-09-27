using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ColorFeature: ShamsiAuditableEntity
    {
        public int Id { get; set; }

        // Normalized key: "1|2|7"
        public required string FeatureIDs { get; set; }
        public required string FeatureName { get; set; }
        public int BrandId { get; set; }
        public int SupplierId { get; set; }
        public int DisplayOrder { get; set; }
        public required string Color { get; set; }
    }
}
