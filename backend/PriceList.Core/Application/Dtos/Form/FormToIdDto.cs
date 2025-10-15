using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Form;

public sealed record FormToIdDto(int Id,
    int BrandId,
    int CategoryId, 
    int ProductGroupId,
    int ProductTypeId,
    int SupplierId,
    int Rows,
    int Column
    );