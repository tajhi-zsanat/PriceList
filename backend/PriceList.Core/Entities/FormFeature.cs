using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormFeature : ShamsiAuditableEntity
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<FormRow> Rows { get; set; } = new List<FormRow>();
    }
}
