using PriceList.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Entities
{
    public class ErrorLog : ShamsiAuditableEntity
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? Path { get; set; }
        public string? UserName { get; set; }
    }
}
