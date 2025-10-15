using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormRowFeature : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public int RowId { get; set; }
        public FormRow Row { get; set; } = default!;

        public int FeatureId { get; set; }
        public Feature Feature { get; set; } = default!;

        // Optional: if a row’s feature *also* carries a value (e.g., “Color = #FF0000”)
        public string? Value { get; set; } // store normalized text; interpret via Feature.ValueKind
    }
}
