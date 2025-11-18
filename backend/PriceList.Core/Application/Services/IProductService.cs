using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface IProductService
    {
        Task<FormCellsScrollResponseDto> GetProducts(
            int category,
            int group,
            int brand,
            int skip,
            int take,
            CancellationToken ct);
    }
}
