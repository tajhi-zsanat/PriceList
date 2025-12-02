using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormViewRepository : IGenericRepository<FormView>
    {
        Task<bool> HasUserViewedFormAsync(
            int formId,
            string viewerKey,
            CancellationToken ct = default);

        Task<List<PopularFormDto>> GetTopPopularFormsAsync(int topCount, CancellationToken ct = default);
    }
}
