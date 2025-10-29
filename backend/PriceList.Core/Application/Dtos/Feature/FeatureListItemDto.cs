using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Feature;

public record FeatureListItemDto(
    int Id,
    string? Title);
