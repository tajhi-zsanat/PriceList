using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public sealed class FormProductType : ShamsiAuditableEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; } = null!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;

        public string? Color{ get; set; }
        public int DisplayOrder { get; set; }
    }
}
