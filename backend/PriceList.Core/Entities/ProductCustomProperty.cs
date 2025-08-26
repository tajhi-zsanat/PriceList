using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductCustomProperty
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string Key { get; set; } = null!;    // e.g. "Condition"
        public string Value { get; set; } = null!;  // e.g. "New"
    }
}
