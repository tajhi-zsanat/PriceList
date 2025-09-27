using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Supplier : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? SupplierValueId { get; set; }
        public int DisplayOrder { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<ProductTypeFeature> ProductTypeFeatures { get; set; } = new List<ProductTypeFeature>();
    }
}
