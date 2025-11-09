using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormRow : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public int RowIndex { get; set; } 

        public int FormId { get; set; }
        public Form Form { get; set; } = default!;

        public int? FormFeatureId { get; set; }
        public FormFeature? FormFeature { get; set; } = default!;


        public ICollection<FormCell> Cells { get; set; } = new List<FormCell>();

    }
}
