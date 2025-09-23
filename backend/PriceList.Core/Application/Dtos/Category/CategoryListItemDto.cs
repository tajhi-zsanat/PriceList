using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Category
{
    public record CategoryListItemDto(int Id, string? Name, string? ImagePath);
}
