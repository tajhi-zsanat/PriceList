using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Api.Dtos.ProductType
{
    public class AddTypeToRowsDto
    {
        public int FormId { get; set; }
        public int TypeId { get; set; }
        public IReadOnlyList<int> RowIds { get; set; } = default!;
        public int DisplayOrder { get; set; }
        public string? Color { get; set; }
    }
}
