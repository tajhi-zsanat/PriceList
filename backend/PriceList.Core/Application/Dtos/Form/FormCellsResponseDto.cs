using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public record FormCellsResponseDto(
    List<FormColumnDefDto> Headers,
    List<GridGroupByFeature> Cells
);