using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form
{
    public sealed class RowWithCellsDto
    {
        public int RowId { get; set; }
        public List<CellDto> Cells { get; set; } = new();
    }
}
