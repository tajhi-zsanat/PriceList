using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form
{
    public sealed class CellDto
    {
        public int Id { get; set; }
        public int ColIndex { get; set; }
        public string? Value { get; set; }
    }
}
