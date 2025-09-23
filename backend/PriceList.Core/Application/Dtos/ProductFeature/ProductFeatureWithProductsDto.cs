using PriceList.Core.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.ProductFeature
{
    public sealed class ProductFeatureWithProductsDto
    {
        //public int[] FeatureIds { get; set; } = System.Array.Empty<int>();
        public string Title { get; set; } = "";
        public List<ProductListItemDto> Products { get; set; } = new();
        public string? FeatureColor { get; set; }
    }
}
