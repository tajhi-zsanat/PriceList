using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record FormColumnDefDto(
    int FormId,
    int Index,
    string Kind,
    string Type,
    string? Key,
    string? Title
);