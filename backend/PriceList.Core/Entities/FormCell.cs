using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormCell : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public int RowId { get; set; }
        public FormRow Row { get; set; } = default!;

        public int ColIndex { get; set; } // still 0-based
        public string? Value { get; set; }
    }

    public enum FeatureValueKind { Text, Number, Boolean, Select, Color }
}
