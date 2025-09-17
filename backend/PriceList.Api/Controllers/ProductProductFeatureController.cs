using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using System.Linq;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductProductFeatureController(IUnitOfWork uow) : ControllerBase
    {
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ProductProductFeatureInputDto dto, CancellationToken ct = default)
        {
            // 1) Basic validation
            if (dto is null)
                return BadRequest("بدنه درخواست خالی است.");

            // Expecting dto.ProductId and dto.productFeatureId as lists of ints
            if (dto.ProductId is null || dto.productFeatureId is null)
                return BadRequest("شناسه‌های محصول و ویژگی الزامی هستند.");

            // 2) Clean & dedupe
            var productIds = dto.ProductId.Where(id => id > 0).ToHashSet();
            var featureIds = dto.productFeatureId.Where(id => id > 0).ToHashSet();

            if (productIds.Count == 0 || featureIds.Count == 0)
                return BadRequest("لیست معتبر از شناسه‌های محصول/ویژگی ارسال نشده است.");

            // 3) Ensure all products exist
            var existingProductIds = await uow.Products
                .ListAsync(
                predicate: (p => productIds.Contains(p.Id)),
                selector: (p => p.Id),
                asNoTracking: true
                );

            var missingProducts = productIds.Except(existingProductIds).ToList();
            if (missingProducts.Count > 0)
                return NotFound($"محصول(ها) با شناسه‌های {string.Join(", ", missingProducts)} یافت نشد.");

            // 4) Ensure all features exist (USE ProductFeatures repo, not Products)
            var existingFeatureIds = await uow.Features.ListAsync(
                predicate: (p => featureIds.Contains(p.Id)),
                selector: (p => p.Id),
                asNoTracking: true
                );


            var missingFeatures = featureIds.Except(existingFeatureIds).ToList();
            if (missingFeatures.Count > 0)
                return NotFound($"ویژگی(ها) با شناسه‌های {string.Join(", ", missingFeatures)} یافت نشد.");

            // 5) Skip already existing pairs
            var existingPairs = await uow.ProductProductFeatures
                .ListAsync(
                predicate: (ppf => productIds.Contains(ppf.ProductId) && featureIds.Contains(ppf.ProductFeatureId)),
                selector: (ppf => new { ppf.ProductId, ppf.ProductFeatureId }),
                asNoTracking: true
                );

            var existingSet = existingPairs
                .Select(x => (x.ProductId, x.ProductFeatureId))
                .ToHashSet();

            // 6) Build new links (cartesian product)
            var toInsert = new List<ProductProductFeature>();
            foreach (var pid in productIds)
            {
                foreach (var fid in featureIds)
                {
                    if (existingSet.Contains((pid, fid)))
                        continue;

                    toInsert.Add(new ProductProductFeature
                    {
                        ProductId = pid,
                        ProductFeatureId = fid
                    });
                }
            }

            // Nothing new to insert
            if (toInsert.Count == 0)
                return NoContent();

            // 7) Save
            await uow.ProductProductFeatures.AddRangeAsync(toInsert, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                // Requires a unique index on (ProductId, ProductFeatureId) at DB level.
                // If a race condition inserted the same pair just now, report 409.
                return Conflict("برخی از لینک‌های محصول/ویژگی از قبل وجود داشتند.");
            }

            return NoContent();
        }
    }
}
