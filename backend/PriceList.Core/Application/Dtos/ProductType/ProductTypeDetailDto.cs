using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.ProductType
{
    public sealed record ProductTypeDetailDto(
        int Id, string Name, string? ImagePath, int GroupId, IReadOnlyList<int> Features, int DisplayOrder);
}
