using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormFeatureRepository : IGenericRepository<FormFeature>
    {
        Task<(List<GridGroupByFeature> Groups, int TotalRows)> GroupRowsAndCellsByTypePagedAsync(
            int formId,
            int userId,
            int page,
            int pageSize,
            CancellationToken ct);

        Task<(IReadOnlyList<GridGroupByFeature> Groups, int TotalRows)> GroupRowsAndCellsByTypeScrollAsync(
            int formId,
            int skip,
            int take,
            CancellationToken ct);
    }
}
