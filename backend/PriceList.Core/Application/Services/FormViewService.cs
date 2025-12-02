using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public class FormViewService : IFormViewService
    {
        private readonly IUnitOfWork _uow;

        public FormViewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task RegisterViewAsync(
        int formId,
        string viewerKey,
        string? ip,
        string? userAgent,
        CancellationToken ct)
        {
            var exists = await _uow.FormViews
                .AnyAsync(v => v.FormId == formId && v.ViewerKey == viewerKey, ct);

            if (exists)
                return;

            await _uow.FormViews.AddAsync(new FormView
            {
                FormId = formId,
                ViewerKey = viewerKey,
                IpAddress = ip,
                UserAgent = userAgent,
                ViewedAt = DateTime.UtcNow
            }, ct);

            await _uow.SaveChangesAsync(ct);
        }

        public async Task<List<PopularFormDto>> GetTopPopularForms(int topCount, CancellationToken ct = default)
        {
            // Using FromSqlInterpolated to pass parameter safely
            var result = await _uow.FormViews.GetTopPopularFormsAsync(topCount, ct);

            return result;
        }
    }
}