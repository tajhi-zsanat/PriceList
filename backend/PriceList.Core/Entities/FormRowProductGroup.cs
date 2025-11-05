using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public sealed class FormRowProductGroup : ShamsiAuditableEntity
    {
        public int FormId { get; set; }
        public int FormRowId { get; set; }
        public FormRow FormRow { get; set; } = null!;

        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; } = null!;
    }
}
