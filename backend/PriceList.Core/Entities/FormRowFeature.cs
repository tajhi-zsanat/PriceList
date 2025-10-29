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

        public int? DisplayOrder { get; set; }

        public string? Color { get; set; }
    }
}
