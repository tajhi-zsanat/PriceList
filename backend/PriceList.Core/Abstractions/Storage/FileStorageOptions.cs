using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Storage
{
    public class FileStorageOptions
    {
        // e.g., C:\Sites\PriceList\wwwroot\uploads or /var/price-list/uploads
        public string PhysicalRoot { get; set; } = default!;
        // e.g., "/uploads"
        public string RequestPath { get; set; } = "/uploads";
    }
}
