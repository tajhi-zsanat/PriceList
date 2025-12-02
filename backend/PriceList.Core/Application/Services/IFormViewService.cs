using PriceList.Core.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface IFormViewService
    {
        Task RegisterViewAsync(
        int formId,
        string viewerKey,         
        string? ip,
        string? userAgent,
        CancellationToken ct);

        Task<List<PopularFormDto>> GetTopPopularForms(int topCount, CancellationToken ct = default);
    }
}
