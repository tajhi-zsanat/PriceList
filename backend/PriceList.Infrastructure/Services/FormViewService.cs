using Microsoft.Extensions.Caching.Memory;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Services;
using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Services
{
    public class FormViewService : IFormViewService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemoryCache _cache;

        private const string PopularFormsCacheKeyPrefix = "popular_forms_top_";

        public FormViewService(IUnitOfWork uow, IMemoryCache cache)
        {
            _uow = uow;
            _cache = cache;
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
            var cacheKey = $"{PopularFormsCacheKeyPrefix}{topCount}";

            var lastChange = await _uow.Forms.GetLastFormOrViewUpdatedAsync(ct);

            if (_cache.TryGetValue(cacheKey, out PopularCacheEntry? entry) && entry is not null)
            {
                if (entry.LastChange >= lastChange)
                    return entry.Data;
            }

            var result = await _uow.FormViews.GetTopPopularFormsAsync(topCount, ct);

            var newEntry = new PopularCacheEntry(result, lastChange);

            _cache.Set(cacheKey, newEntry); 

            return result;
        }
    }
}