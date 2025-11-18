using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record FormCellsPageResponseDto(
    string? FormTitle,
    IReadOnlyList<FormColumnDefDto> Headers,
    IReadOnlyList<GridGroupByFeature> Cells,
    PaginationMeta Meta);
