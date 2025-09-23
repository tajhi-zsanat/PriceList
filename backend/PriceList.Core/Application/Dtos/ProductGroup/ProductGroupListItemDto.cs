using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.ProductGroup
{
    public record ProductGroupListItemDto(int Id, string? Name, string? ImagePath);
}
