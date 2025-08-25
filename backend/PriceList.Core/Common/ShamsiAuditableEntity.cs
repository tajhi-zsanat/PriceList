using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Common
{
    public class ShamsiAuditableEntity
    {
        // Persian (Shamsi) text fields
        public string? CreateDate { get; set; }   // yyyy/MM/dd
        public string? CreateTime { get; set; }   // HHmm
        public string? UpdateDate { get; set; }   // yyyy/MM/dd
        public string? UpdateTime { get; set; }   // HHmm

        // UTC timestamps
        public DateTime CreateDateAndTime { get; set; }
        public DateTime UpdateDateAndTime { get; set; }
    }
}
