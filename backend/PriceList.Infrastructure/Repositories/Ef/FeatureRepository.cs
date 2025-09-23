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
using System.Diagnostics;
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
            var filtered = _db.Products
                .AsNoTracking()
                .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
                .Where(p => !groupId.HasValue || p.ProductGroupId == groupId.Value)
                .Where(p => !typeId.HasValue || p.ProductTypeId == typeId.Value)
                .Where(p => !brandId.HasValue || p.BrandId == brandId.Value)
                .Where(p => !supplierId.HasValue || p.SupplierId == supplierId.Value);

            if (!string.IsNullOrWhiteSpace(productSearch))
            {
                var term = productSearch.Trim();
                filtered = filtered.Where(p =>
                    (p.Model != null && p.Model.Contains(term)) ||
                    (p.Description != null && p.Description.Contains(term)));
            }

            var totalCount = await filtered.CountAsync(ct);

            var pageIds = await filtered
                .OrderBy(p => p.Id)
                .Select(p => p.Id)
                .ToListAsync(ct);

            if (pageIds.Count == 0)
            {
                return new ScrollResult<ProductFeatureWithProductsDto>
                {
                    Items = System.Array.Empty<ProductFeatureWithProductsDto>(),
                    Skip = skip,
                    Take = take,
                    ReturnedCount = 0,
                    TotalCount = totalCount
                };
            }

            var productswithFeature = await _db.ProductFeatures
                .Where(cf => pageIds.Contains(cf.ProductId))
                .Select(cf => cf.ProductId)
                .Distinct()
                .ToListAsync();

            var productsWithoutFeature = pageIds.Except(productswithFeature).ToList();

            var containers = new Dictionary<string, List<int>>();

            var groupProductWithFeature = await _db.ProductFeatures
                .Where(pf => pageIds.Contains(pf.ProductId))
                .GroupBy(pf => pf.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    FeatureIds = FeatureKeyHelper.Build(g.Select(p => p.FeatureId)),
                }).ToListAsync();

            var ProductsIDs = new List<int>();

            foreach (var groupProduct in groupProductWithFeature)
            {
                if (containers.Keys.Contains(groupProduct.FeatureIds))
                {
                    containers[groupProduct.FeatureIds].Add(groupProduct.ProductId);
                    ProductsIDs.Add(groupProduct.ProductId);
                }
                else
                {
                    containers.Add(groupProduct.FeatureIds, []);
                    containers[groupProduct.FeatureIds].Add(groupProduct.ProductId);
                    ProductsIDs.Add(groupProduct.ProductId);
                }
            }

            containers.Add("Others", productsWithoutFeature);

            foreach (var product in productsWithoutFeature)
            {
                ProductsIDs.Add(product);
            }

            var products = await _db.Products
                .Where(p => ProductsIDs.Contains(p.Id))
                .Select(ProductMappings.ToListItem)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var result1 = new List<ProductFeatureWithProductsDto>();

            var colorDict = new Dictionary<string, string>();
            var colorIDs = new List<int>();

            colorIDs = groupProductWithFeature
           .SelectMany(gp => gp.FeatureIds.Split('|'))   // flatten all parts
           .Select(int.Parse)                            // convert each part to int
           .ToList();

            foreach (var kvp in containers)
            {
                result1.Add(new ProductFeatureWithProductsDto
                {
                    Title = kvp.Key,
                    Products = products.Where(p => kvp.Value.Contains(p.Id)).ToList(),
                });
            }

            var colorFeature = await _db.ColorFeatures
                .Where(cf => cf.BrandId == brandId && cf.SupplierId == supplierId)
                .OrderBy(cf => cf.DisplayOrder)
                .Select(x => new { color = x.Color, featureIDs = x.FeatureIDs })
                .ToListAsync();

            var result2 = new List<ProductFeatureWithProductsDto>();


            foreach (var cf in colorFeature)
            {
                foreach (var r1 in result1)
                {
                    if (cf.featureIDs == r1.Title && r1.Products.Any())
                    {
                        result2.Add(r1);
                    }
                }
            }

            //throw new NullReferenceException();

            // 10) Return scroll page
            return new ScrollResult<ProductFeatureWithProductsDto>
            {
                Items = result2,
                Skip = skip,
                Take = take,
                ReturnedCount = 900000,
                TotalCount = totalCount
            };
        }

    }
}
