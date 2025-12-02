using PriceList.Core.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record PopularCacheEntry(
    List<PopularFormDto> Data,
    DateTime LastChange
);
