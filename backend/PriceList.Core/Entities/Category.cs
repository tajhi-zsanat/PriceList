using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Category : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Product> Products { get; set; } = [];
        public ICollection<Form> Forms { get; set; } = [];
    }
}
