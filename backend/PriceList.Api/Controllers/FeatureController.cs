using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using PriceList.Api.Dtos.Category;
using PriceList.Api.Dtos.Feature;
using PriceList.Api.Dtos.ProductFeature;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Category;
using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Application.Dtos.ProductType;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Entities;
using System;
using System.Text.RegularExpressions;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FeatureController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-category")]
        [ProducesResponseType(typeof(List<FeatureListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FeatureListItemDto>>> GetByCategory(
          [FromQuery] int formId,
          CancellationToken ct)
        {
            if (formId <= 0)
                return BadRequest("شناسه گروه کالا نامعتبر است.");

            var formIds = await uow.Forms.FirstOrDefaultAsync(
                predicate: f => f.Id == formId,
                selector: f => new { f.ProductTypeId, f.SupplierId },
                ct: ct);

            if (formIds is null)
                return NotFound("فرم مورد نظر یافت نشد.");

            var features = await uow.ProductTypeFeatures.ListAsync(
                predicate: pf => pf.ProductTypeId == formIds.ProductTypeId
                               && pf.SupplierId == formIds.SupplierId,
                selector: pf => new FeatureListItemDto(
                    pf.Feature.Id,
                    pf.Feature.Name),
                ct: ct);

            return Ok(features);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(
            [FromBody] FeatureCreateDto form,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var requestedRowIds = form.RowIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            var requestedFeatureIds = form.FeatureIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (requestedRowIds.Count == 0)
                return BadRequest("شناسه ردیف معتبر ارسال نشده است.");

            if (requestedFeatureIds.Count == 0)
                return BadRequest("شناسه ویژگی معتبر ارسال نشده است.");

            if (form.DisplayOrder <= 0)
                return BadRequest("ترتیب نمایش باید بزرگتر از 0 باشد.");

            var existingRowIds = await uow.FormRows.ListAsync(
                predicate: r => requestedRowIds.Contains(r.Id),
                selector: r => r.Id,
                ct: ct);

            if (existingRowIds.Count != requestedRowIds.Count)
                return BadRequest("برخی از ردیف‌های ارسال‌شده یافت نشد.");

            var existingFeatureIds = await uow.Features.ListAsync(
                predicate: f => requestedFeatureIds.Contains(f.Id),
                selector: f => f.Id,
                ct: ct);

            if (existingFeatureIds.Count != requestedFeatureIds.Count)
                return NotFound("برخی از ویژگی‌های ارسال‌شده یافت نشد.");

            var existingPairs = await uow.FormRowFeatures.ListAsync(
                predicate: rf => existingRowIds.Contains(rf.RowId) && existingFeatureIds.Contains(rf.FeatureId),
                selector: rf => new { rf.RowId, rf.FeatureId },
                ct: ct);

            var existingPairSet = new HashSet<(int RowId, int FeatureId)>(
                existingPairs.Select(p => (p.RowId, p.FeatureId)));

            var entities = new List<FormRowFeature>(capacity: requestedRowIds.Count * requestedFeatureIds.Count);

            foreach (var rowId in existingRowIds)
            {
                foreach (var featureId in existingFeatureIds)
                {
                    if (existingPairSet.Contains((rowId, featureId)))
                        continue;

                    entities.Add(new FormRowFeature
                    {
                        RowId = rowId,
                        FeatureId = featureId,
                        Color = form.Color,
                        DisplayOrder = form.DisplayOrder,
                    });
                }
            }

            if (entities.Count == 0)
                return Conflict("همه ترکیب‌های (ردیف/ویژگی) از قبل ثبت شده‌اند.");

            await uow.FormRowFeatures.AddRangeAsync(entities, ct);
            await uow.SaveChangesAsync(ct);

            return StatusCode(StatusCodes.Status201Created, new
            {
                created = entities.Count,
                featureIds = existingFeatureIds,
                rows = existingRowIds
            });
        }
    }
}
