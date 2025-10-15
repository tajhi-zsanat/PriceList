using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public record FormMetaDto(int Id, string Title, int Rows, List<FormColumnDto> Columns);

public record FormColumnDto(
    int Id, int Index, string Key, string Title, ColumnKind Kind, ColumnType Type,
    bool Required, int? WidthPx, int? FeatureId);

public record CellFeatureValueDto(
    int FeatureId, FeatureValueKind Kind,
    string? TextValue, bool? BoolValue, string? OptionKey, string? ColorHex);
