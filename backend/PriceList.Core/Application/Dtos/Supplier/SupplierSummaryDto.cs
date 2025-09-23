using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Supplier
{
    public sealed record SupplierSummaryDto(
        int SupplierId,
        string SupplierName,
        int ProductCount
    );
}
