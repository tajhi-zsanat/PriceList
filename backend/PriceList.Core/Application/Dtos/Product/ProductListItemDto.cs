using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Product
{
    public sealed record ProductListItemDto(
        int Id,
        string? Model,
        string? Description,
        string? DocumentPath,
        long Price,
        int Number,
        IReadOnlyList<string> ProductImages,
        IReadOnlyList<ProductHeaderItemDto> ProductproductHeaders
    );
}
