using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = null!;

        public bool IsMain { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
