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

        public int FormId { get; set; }
        public Form Form { get; set; } = default!;

        public int RowIndex { get; set; } // 0-based, stable logical row id within a form

        public ICollection<FormCell> Cells { get; set; } = new List<FormCell>();

        public ICollection<FormRowProductGroup> RowProductGroups { get; set; } = [];
        //public ICollection<FormRowFeature> Features { get; set; } = new List<FormRowFeature>();
    }
}
