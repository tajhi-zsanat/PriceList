using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Product;
public sealed record ProductsWithHeaders(
    IReadOnlyList<HeaderListItemDto> Headers,
    ScrollResult<ProductFeatureWithProductsDto> Result
);
