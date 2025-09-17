using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Product : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public string? Description { get; set; }
        public long Price { get; set; }
        public int Number { get; set; }
        public ICollection<ProductImage> Images { get; set; } = [];
        public string? DocumentPath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; } = null!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        //public string? UserId { get; set; } = null!;

        //public ApplicationUser? User { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; } = null!;

        public ICollection<ProductCustomProperty> CustomProperties { get; set; } = new List<ProductCustomProperty>();
        public ICollection<productHeader> productHeaders { get; set; } = new List<productHeader>();

        public ICollection<ProductProductFeature> ProductFeatures { get; set; } = new List<ProductProductFeature>();
    }
}
