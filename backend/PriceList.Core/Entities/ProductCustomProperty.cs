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

        public int productHeaderId { get; set; }
        public productHeader productHeader { get; set; } = null!;

        public string Value { get; set; } = null!; 
    }
}
