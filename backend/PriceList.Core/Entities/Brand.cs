using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Brand : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImagePath { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        //public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}
