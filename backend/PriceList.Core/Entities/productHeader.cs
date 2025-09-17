using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class productHeader : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public required string Key { get; set; }

        public ICollection<ProductCustomProperty> CustomProperties { get; set; } = new List<ProductCustomProperty>();
    }
}
