using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.Header;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Entities;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HeaderController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-categories")]
        [ProducesResponseType(typeof(List<HeaderListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<HeaderListItemDto>>> GetByCategory(
            [FromQuery] int brandId,
            [FromQuery] int typeId,
            CancellationToken ct = default)
        {
            if (typeId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var headers = await uow.Header.ListAsync(
                predicate: (ph => ph.BrandId == brandId && ph.ProductTypeId == typeId),
                selector: ProductHeaderMappings.ToListItem,
                asNoTracking: true,
                ct: ct
                );

            return Ok(headers);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] List<HeaderInputDto> dto, CancellationToken ct = default)
        {
            if (dto is null || dto.Count == 0)
                return NoContent();

            // Normalize & basic validation
            var cleaned = dto
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Select(x => new { Name = x.Name.Trim(), x.BrandId, TypeId = x.TypeId })
                .ToList();

            foreach (var item in cleaned)
            {
                if (item.BrandId <= 0) return BadRequest("شناسه برند نامعتبر است.");
                if (item.TypeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است."); // fixed message
            }

            // Existence checks (simple loop; batch if needed)
            foreach (var item in cleaned)
            {
                var brandExists = await uow.Brands.AnyAsync(b => b.Id == item.BrandId, ct);
                if (!brandExists) return NotFound($"برند با شناسه {item.BrandId} یافت نشد.");

                var typeExists = await uow.ProductTypes.AnyAsync(t => t.Id == item.TypeId, ct);
                if (!typeExists) return NotFound($"نوع کالا با شناسه {item.TypeId} یافت نشد."); // was a small bug before
            }

            // 1) remove duplicates inside the same request
            var uniqueInPayload = cleaned
                .GroupBy(x => (x.BrandId, x.TypeId, x.Name.Trim()))
                .Select(g => g.First())
                .ToList();

            // 2) fetch existing pairs in one round-trip
            var brandIds = uniqueInPayload.Select(x => x.BrandId).Distinct().ToList();
            var typeIds = uniqueInPayload.Select(x => x.TypeId).Distinct().ToList();
            var Names = uniqueInPayload.Select(x => x.Name).Distinct().ToList();

            var existingPairs = await uow.Header
                .ListAsync(predicate: (ph => brandIds.Contains(ph.BrandId) &&
                typeIds.Contains(ph.ProductTypeId) &&
                Names.Contains(ph.Key)),
                selector: (ph => new { ph.BrandId, ph.ProductTypeId }),
                asNoTracking: true,
                ct: ct
                );

            var existingSet = existingPairs
                .Select(p => (p.BrandId, p.ProductTypeId))
                .ToHashSet();

            // 3) build only the missing inserts
            var toInsert = uniqueInPayload
                .Where(x => !existingSet.Contains((x.BrandId, x.TypeId)))
                .Select(x => new Header
                {
                    Key = x.Name,
                    BrandId = x.BrandId,
                    ProductTypeId = x.TypeId
                })
                .ToList();

            if (toInsert.Count == 0)
                return NoContent(); // nothing new

            // Save
            await uow.Header.AddRangeAsync(toInsert, ct);
            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                // Rare race: another request inserted the same pair after our check.
                // Return 409 or convert to NoContent depending on your preference.
                return Conflict("آیتم(های) تکراری برای ترکیب برند/نوع شناسایی شد.");
            }

            return NoContent(); // you prefer no body
        }
    }
}
