using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form
{
    public sealed class GridGroupByFeature
    {
        // e.g., ["Color","Size"] — empty for rows with no features
        public string? FeatureName { get; set; }
        public int FeatureId { get; set; }
        public string? Color { get; set; }
        public List<RowWithCellsDto> Rows { get; set; } = new();
        public int Count => Rows.Count;
    }
}
