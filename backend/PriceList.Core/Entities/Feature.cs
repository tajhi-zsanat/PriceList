using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Feature : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        //public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public ICollection<ProductTypeFeature> ProductTypeFeatures { get; set; } = new List<ProductTypeFeature>();

        public ICollection<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();
    }
}
