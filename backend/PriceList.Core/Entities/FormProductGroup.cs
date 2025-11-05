using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public sealed class FormProductGroup : ShamsiAuditableEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; } = null!;

        public int ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; } = null!;

        public string? Color{ get; set; }
        public int DisplayOrder { get; set; }
    }
}
