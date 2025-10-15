using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormColumnDef : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public Form Form { get; set; } = default!;

        public int Index { get; set; }                 // 0-based position in the grid
        public ColumnKind Kind { get; set; }           // Static or Dynamic
        public ColumnType Type { get; set; }           // how to render/edit
        public string Key { get; set; } = default!;    // machine key (e.g., "rowNo","sku","price")
        public string Title { get; set; } = default!;
        public bool Required { get; set; }
        public int? WidthPx { get; set; }
    }
}
