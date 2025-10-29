using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record PaginationMeta(
    int Page,
    int PageSize,
    int TotalRows,
    int TotalPages,
    bool HasPrev,
    bool HasNext);