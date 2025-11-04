using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public sealed class FormRowProductType : ShamsiAuditableEntity
    {
        public int FormId { get; set; }     
        public int FormRowId { get; set; }
        public FormRow FormRow { get; set; } = null!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;
    }
}
