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
        public string? FeaturesIDs { get; set; }
        public string Title { get; set; } = "";
        public List<ProductListItemDto> Products { get; set; } = new();
        public string? FeatureColor { get; set; }
    }
}
