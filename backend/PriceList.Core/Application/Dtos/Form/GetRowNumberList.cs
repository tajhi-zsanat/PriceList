using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

    public record GetRowNumberList(int Id, int RowNumber, string? FeatureName);
