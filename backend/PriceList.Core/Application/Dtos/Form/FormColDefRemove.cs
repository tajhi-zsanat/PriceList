using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form
{
    public class FormColDefRemove
    {
        public int FormId { get; set; }
        public required int Index { get; set; }
    }
}
