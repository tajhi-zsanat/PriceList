using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record FormCellsListItemDto(
    int Id,
    int ColIndex,
    string? Value
    );
