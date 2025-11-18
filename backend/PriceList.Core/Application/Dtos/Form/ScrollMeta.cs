using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;
public sealed record ScrollMeta(
    string LastUpdate,
    int TotalRows,
    int Skip,
    int Take,
    int ReturnedCount,
    int TotalCount,
    bool TotalProductCount,
    bool HasMore);
