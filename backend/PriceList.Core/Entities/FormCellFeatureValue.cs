using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormCellFeatureValue : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public int CellId { get; set; }
        public FormCell Cell { get; set; } = default!;
        public int FeatureId { get; set; }        
    }
}
