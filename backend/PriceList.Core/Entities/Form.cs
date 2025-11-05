using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class Form : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public string? FormTitle { get; set; }

        public int DisplayOrder { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int Rows { get; set; }
        public int MinCols { get; set; } = 5;          
        public int MaxCols { get; set; } = 8;         

        public ICollection<FormColumnDef> Columns { get; set; } = [];

        public ICollection<FormProductGroup> FormProductGroups { get; set; } = [];

        public ICollection<FormRow> FormRows { get; set; } = [];
    }

    public enum ColumnKind
    {
        Static,
        Dynamic,
        Fixed
    }

    public enum ColumnType
    {
        Checkbox,
        Rowno,
        Image,
        File,
        MultilineText,
        Select,
        Custom1,
        Custom2,
        Custom3,
        Text,
        Price,
        More,
        NotAssign
    }
}
