using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductType : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public int ProductGroupId { get; set; }

        public ProductGroup ProductGroup { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();

        //public ICollection<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();

        public ICollection<ProductTypeFeature> ProductTypeFeatures { get; set; } = new List<ProductTypeFeature>();

        //public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
