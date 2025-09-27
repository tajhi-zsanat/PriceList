using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FeatureRepository : GenericRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(AppDbContext db, ILogger<Feature> logger)
            : base(db, logger)
        {
        }

        /// <summary>
        /// Infinite-scroll version (skip/take) with grouping by exact feature-set.
        /// Slices the final product stream across grouped buckets so the client
        /// can lazy-load with IntersectionObserver or similar.
        /// </summary>
        public async Task<ScrollResult<ProductFeatureWithProductsDto>> ListFeaturesWithProductsScrollAsync(
            int skip,
            int take,
            int? categoryId = null,
            int? groupId = null,
            int? typeId = null,
            int? brandId = null,
            int? supplierId = null,
            string? productSearch = null,
            CancellationToken ct = default)
        {
            // 0) Base filter
            IQueryable<Product> filtered = _db.Products.AsNoTracking()
                .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                .Where(p => !groupId.HasValue || p.ProductGroupId == groupId.Value)
                .Where(p => !typeId.HasValue || p.ProductTypeId == typeId.Value)
                .Where(p => !brandId.HasValue || p.BrandId == brandId.Value)
                .Where(p => !supplierId.HasValue || p.SupplierId == supplierId.Value);

            if (!string.IsNullOrWhiteSpace(productSearch))
            {
                var term = $"%{productSearch.Trim()}%";
                filtered = filtered.Where(p =>
                    (p.Description != null && EF.Functions.Like(p.Description, term)));
            }

            var totalCount = await filtered.CountAsync(ct);
            if (totalCount == 0)
            {
                return new ScrollResult<ProductFeatureWithProductsDto>
                {
                    Items = Array.Empty<ProductFeatureWithProductsDto>(),
                    Skip = skip,
                    Take = take,
                    ReturnedCount = 0,
                    TotalProductCount = 0,
                    TotalCount = 0
                };
            }

            // 1) Candidate product IDs
            var allProductIds = await filtered
                .OrderBy(p => p.Id)
                .Select(p => p.Id)
                .ToListAsync(ct);

            if (allProductIds.Count == 0)
            {
                return new ScrollResult<ProductFeatureWithProductsDto>
                {
                    Items = Array.Empty<ProductFeatureWithProductsDto>(),
                    Skip = skip,
                    Take = take,
                    ReturnedCount = 0,
                    TotalProductCount = 0,
                    TotalCount = 0
                };
            }

            var allProductIdSet = new HashSet<int>(allProductIds);

            // 2) Product → FeatureKey
            var prodToKey = await _db.ProductFeatures
                .Where(pf => allProductIdSet.Contains(pf.ProductId))
                .GroupBy(pf => pf.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    FeatureKey = FeatureKeyHelper.Build(g.Select(x => x.FeatureId))
                })
                .ToListAsync(ct);

            var productsWithFeatureIds = new HashSet<int>(prodToKey.Select(x => x.ProductId));
            var productsWithoutFeature = allProductIds.Where(id => !productsWithFeatureIds.Contains(id));

            // 3) Buckets: FeatureKey → productId list
            var buckets = new Dictionary<string, List<int>>(StringComparer.Ordinal);
            foreach (var r in prodToKey)
            {
                if (!buckets.TryGetValue(r.FeatureKey, out var list))
                {
                    list = new List<int>();
                    buckets[r.FeatureKey] = list;
                }
                list.Add(r.ProductId);
            }
            const string OthersKey = "__OTHERS__";
            buckets[OthersKey] = productsWithoutFeature.ToList();

            // 4) ColorFeatures mapping
            var colorRows = await _db.ColorFeatures
                .Where(cf => (!brandId.HasValue || cf.BrandId == brandId.Value)
                          && (!supplierId.HasValue || cf.SupplierId == supplierId.Value))
                .OrderBy(cf => cf.DisplayOrder)
                .Select(cf => new { cf.FeatureIDs, cf.FeatureName, cf.Color })
                .ToListAsync(ct);

            var keyToTitle = new Dictionary<string, string>(StringComparer.Ordinal);
            var keyToColor = new Dictionary<string, string?>(StringComparer.Ordinal);

            foreach (var r in colorRows)
            {
                if (!keyToTitle.ContainsKey(r.FeatureIDs))
                    keyToTitle[r.FeatureIDs] = r.FeatureName;

                if (!keyToColor.ContainsKey(r.FeatureIDs))
                    keyToColor[r.FeatureIDs] = r.Color;
            }

            // Order: configured keys → remaining keys → Others
            var orderedKeys = new List<string>();
            foreach (var kr in colorRows)
                if (buckets.ContainsKey(kr.FeatureIDs))
                    orderedKeys.Add(kr.FeatureIDs);

            foreach (var k in buckets.Keys)
                if (k != OthersKey && !orderedKeys.Contains(k))
                    orderedKeys.Add(k);

            if (buckets[OthersKey].Count > 0)
                orderedKeys.Add(OthersKey);

            var totalGroupCount = orderedKeys.Count;

            // 5) Page by groups
            var pagedKeys = orderedKeys.Skip(skip).Take(take).ToList();
            if (pagedKeys.Count == 0)
            {
                return new ScrollResult<ProductFeatureWithProductsDto>
                {
                    Items = Array.Empty<ProductFeatureWithProductsDto>(),
                    Skip = skip,
                    Take = take,
                    ReturnedCount = 0,
                    TotalCount = totalGroupCount,
                    TotalProductCount = totalCount
                };
            }

            // 6) Load products for selected groups
            var selectedIds = new HashSet<int>(pagedKeys.SelectMany(k => buckets[k]));
            var products = await _db.Products.AsNoTracking()
                .Where(p => selectedIds.Contains(p.Id))
                .Select(ProductMappings.ToListItem)
                .ToListAsync(ct);

            var productById = products.ToDictionary(x => x.Id);

            // 7) Build result items
            var items = new List<ProductFeatureWithProductsDto>(pagedKeys.Count);
            foreach (var key in pagedKeys)
            {
                var title =
                    key == OthersKey
                        ? "سایر محصولات"
                        : (keyToTitle.TryGetValue(key, out var nm) ? nm : key);

                var ids = buckets[key];
                var groupProducts = new List<ProductListItemDto>(ids.Count);
                foreach (var id in ids)
                    if (productById.TryGetValue(id, out var dto))
                        groupProducts.Add(dto);

                string? featureColor = null;
                if (key != OthersKey && keyToColor.TryGetValue(key, out var c))
                    featureColor = c;

                items.Add(new ProductFeatureWithProductsDto
                {
                    FeaturesIDs = key,
                    Title = title,
                    FeatureColor = featureColor,
                    Products = groupProducts
                });
            }

            return new ScrollResult<ProductFeatureWithProductsDto>
            {
                Items = items,
                Skip = skip,
                Take = take,
                ReturnedCount = items.Count,
                TotalCount = totalGroupCount,
                TotalProductCount = totalCount
            };
        }
    }
}
