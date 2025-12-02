using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class FormView : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public int FormId { get; set; }

        public string ViewerKey { get; set; } = default!; 

        public DateTime ViewedAt { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
