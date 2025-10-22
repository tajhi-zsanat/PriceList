using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form
{
    public sealed class FeatureNameSetGroupDto
    {
        // e.g., ["Color","Size"] — empty for rows with no features
        public List<string> FeatureNames { get; set; } = new();
        public string? FeatureColor { get; set; }
        public List<RowWithCellsDto> Rows { get; set; } = new();
        public int Count => Rows.Count;
    }
}
