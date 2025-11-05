using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.Header;
using PriceList.Api.Dtos.ProductType;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.ProductType;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Application.Services;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Core.Enums;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductTypeController(IUnitOfWork uow, IFileStorage storage) : ControllerBase
    {
        [HttpGet("by-group")]
        [ProducesResponseType(typeof(List<ProductTypeListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProductTypeListItemDto>>> GetByGroup(int groupId, CancellationToken ct)
        {
            if (groupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");

            var list = await uow.ProductTypes.GetByGroupIdAsync(
                groupId,
                selector: ProductTypeMappings.ToListItem,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductTypeListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductTypeListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            var list = await uow.ProductTypes.ListAsync(
                predicate: null,
                selector: ProductTypeMappings.ToListItem,
                asNoTracking: false,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetTypeById")]
        [ProducesResponseType(typeof(ProductTypeListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductTypeListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.ProductTypes.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: ProductTypeMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("نوع کالا یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ProductTypeListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductTypeListItemDto>> Create(
          [FromForm] TypeCreateForm form,
          CancellationToken ct = default)
        {
            var name = form.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (form.GroupId <= 0)
                return BadRequest("شناسه گروه کالا نامعتبر است.");

            var groupExists = await uow.ProductGroups.AnyAsync(g => g.Id == form.GroupId, ct);
            if (!groupExists)
                return NotFound("گروه کالا یافت نشد.");

            var dupExists = await uow.ProductTypes.AnyAsync(
                t => t.ProductGroupId == form.GroupId &&
                     t.Name.Trim().ToUpper() == name.ToUpper(),
                ct);

            if (dupExists)
                return Conflict("نام این نوع کالا در همین گروه تکراری است.");

            if (dupExists)
                return Conflict("اطلاعات وارد شده تکراری می‌باشد.");

            string? imagePath = null;
            if (form.Image is not null && form.Image.Length > 0)
            {
                var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
                var ext = Path.GetExtension(form.Image.FileName);
                if (!allowed.Contains(ext))
                    return BadRequest("فرمت تصویر نامعتبر است.");

                try
                {
                    using var s = form.Image.OpenReadStream();
                    imagePath = await storage.SaveAsync(s, form.Image.FileName, "types", ct);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var featureIds = (form.Features ?? new List<int>()).Distinct().ToArray();

            //if (featureIds.Length > 0)
            //{
            //    var existingFeatureIds = await uow.Features.ListAsync(
            //        predicate: f => featureIds.Contains(f.Id),
            //        asNoTracking: false,
            //        ct: default
            //     );

            //    if (existingFeatureIds.Count != featureIds.Length)
            //        return BadRequest("برخی ویژگی‌های انتخاب‌شده معتبر نیستند.");
            //}

            var entity = new ProductType
            {
                Name = name,
                DisplayOrder = form.DisplayOrder,
                ProductGroupId = form.GroupId,
                ImagePath = imagePath,
            };

            await uow.ProductTypes.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                // Covers race conditions when a unique index exists
                return Conflict("نام گروه کالا تکراری است.");
            }

            //var result = ProductTypeApiMappers.ToListItemDto(entity);
            //return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
            return Created();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var entity = await uow.ProductTypes.GetByIdAsync(id, ct);
            if (entity is null)
                return NotFound("نوع کالا پیدا نشد.");

            try
            {
                uow.ProductTypes.Remove(entity);
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsForeignKeyViolation(ex))
            {
                // if you add this helper similar to IsUniqueViolation
                return Conflict("امکان حذف نوع کالا به دلیل وابستگی وجود ندارد.");
            }

            return NoContent(); // 204 — success, no response body
        }

        [HttpDelete("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken ct = default)
        {
            if (id <= 0) return NotFound("شناسه نوع کالا نامعتبر است.");

            var entity = await uow.ProductTypes.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("شناسه نوع کالا پیدا نشد.");

            if (string.IsNullOrWhiteSpace(entity.ImagePath))
                return NoContent();

            var path = entity.ImagePath;

            // Clear DB reference first to keep data consistent even if file delete fails.
            entity.ImagePath = null;
            await uow.SaveChangesAsync(ct);

            try
            {
                await storage.DeleteAsync(path!, ct);
            }
            catch (Exception ex)
            {
            }

            return NoContent();
        }
    }
}
