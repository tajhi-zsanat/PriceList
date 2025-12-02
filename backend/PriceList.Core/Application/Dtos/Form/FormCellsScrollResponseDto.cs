using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;
public sealed record FormCellsScrollResponseDto(
    int? formId,
    Product Status,
    IReadOnlyList<FormColumnDefDto> Headers,
    IReadOnlyList<GridGroupByFeature> Cells,
    ScrollMeta Meta
    );

public enum Product
{
    Initial,
    FormNotFound,
    MaxColumnsReached,
    AlreadyExists,
    ColumnNotFound,
    InvalidColumn,
    NoContent,
}