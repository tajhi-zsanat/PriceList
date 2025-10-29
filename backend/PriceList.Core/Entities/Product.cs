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

        public int FormId { get; set; }
        public Form Form { get; set; } = null!;

        public int? UnitId { get; set; }
        public Unit? Unit { get; set; } = null!;

        //public ICollection<ProductHeader> ProductHeaders { get; set; } = new List<ProductHeader>();
        //public ICollection<Header> Headers { get; set; } = new List<Header>();

        //public ICollection<ProductFeature> ProductFeatures { get; set; } = new List<ProductFeature>();
    }
}
