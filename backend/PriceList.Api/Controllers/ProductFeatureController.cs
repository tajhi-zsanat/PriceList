using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.ProductFeature;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System.Linq;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFeatureController(IUnitOfWork uow) : ControllerBase
    {
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ProductFeatureAssignDto dto, CancellationToken ct = default)
        {
            if (dto is null)
                return BadRequest("بدنه درخواست خالی است.");

            if (dto.ProductIds is null || dto.FeatureIds is null)
                return BadRequest("شناسه‌های محصول و ویژگی الزامی هستند.");

            var productIds = dto.ProductIds.Where(id => id > 0).ToHashSet();
            var featureIds = dto.FeatureIds.Where(id => id > 0).ToHashSet();

            if (productIds.Count == 0 || featureIds.Count == 0)
                return BadRequest("لیست معتبر از شناسه‌های محصول/ویژگی ارسال نشده است.");

            if (dto.BrandId <= 0)
                return BadRequest("شناسه برند نامعتبر می‌باشد.");

            var existingBrandId = await uow.Brands.GetByIdAsync(dto.BrandId, ct);

            if (existingBrandId == null)
                return BadRequest("شناسه برند نامعتبر می‌باشد.");

            // Validate products exist
            var existingProductIds = await uow.Products.ListAsync(
                predicate: p => productIds.Contains(p.Id),
                selector: p => p.Id,
                asNoTracking: true,
                ct: ct
            );
            var missingProducts = productIds.Except(existingProductIds).ToList();
            if (missingProducts.Count > 0)
                return NotFound($"محصول(ها) با شناسه‌های {string.Join(", ", missingProducts)} یافت نشد.");

            // Validate features exist
            var existingFeatureIds = await uow.Features.ListAsync(
                predicate: f => featureIds.Contains(f.Id),
                selector: f => f.Id,
                asNoTracking: true,
                ct: ct
            );
            var missingFeatures = featureIds.Except(existingFeatureIds).ToList();
            if (missingFeatures.Count > 0)
                return NotFound($"ویژگی(ها) با شناسه‌های {string.Join(", ", missingFeatures)} یافت نشد.");

            var removeProductFeatures = await uow.ProductFeatures.ListAsync(
                predicate: (pf => productIds.Contains(pf.ProductId)),
                asNoTracking: false,
                ct: ct);

            uow.ProductFeatures.RemoveRange(removeProductFeatures);

            // Build new links (cartesian product minus existing)
            var toInsert = new List<ProductFeature>();
            foreach (var pid in productIds)
                foreach (var fid in featureIds)
                    toInsert.Add(new ProductFeature
                    {
                        ProductId = pid,
                        FeatureId = fid,
                    });

            // If nothing to insert and you also don't need a ColorFeature row, exit early
            if (toInsert.Count == 0 && string.IsNullOrWhiteSpace(dto.FeatureColor))
                return NoContent();

            // Stage inserts
            if (toInsert.Count > 0)
                await uow.ProductFeatures.AddRangeAsync(toInsert, ct);

            var isExistFeatureColor = await uow.ColorFeatures.GetByFeaturesAsync(string.Join(",", featureIds), dto.BrandId, dto.SupplierId, ct);

            if (!isExistFeatureColor)
            {
                var colorFeature = new ColorFeature
                {
                    FeatureIDs = FeatureKeyHelper.Build(featureIds),
                    // If you want the chosen color recorded:
                    Color = dto.FeatureColor,
                    BrandId = dto.BrandId,
                    SupplierId = dto.SupplierId,
                };
                await uow.ColorFeatures.AddAsync(colorFeature, ct);
            }

            try
            {
                // ✅ Single commit point
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                // Requires unique index on (ProductId, ProductFeatureId) to be reliable against races.
                return Conflict("برخی از لینک‌های محصول/ویژگی از قبل وجود داشتند.");
            }

            return NoContent();
        }
    }
}
