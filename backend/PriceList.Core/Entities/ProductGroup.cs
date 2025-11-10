using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductGroup : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        //public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public ICollection<Form> Forms { get; set; } = [];
    }
}
