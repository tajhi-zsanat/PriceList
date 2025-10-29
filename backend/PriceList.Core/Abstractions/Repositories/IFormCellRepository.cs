using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormCellRepository : IGenericRepository<FormCell>
    {
        Task<(List<FeatureNameSetGroupDto> Groups, int TotalRows)> GroupRowsAndCellsByFeatureNamesPagedAsync(
        int formId,
        int page,
        int pageSize,
        CancellationToken ct);
    }
}
