using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.ProductGroup;
using PriceList.Api.Dtos.ProductType;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Application.Dtos.ProductType;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Application.Services;
using PriceList.Core.Entities;
using PriceList.Core.Enums;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductGroupController(IUnitOfWork uow, IFileStorage storage, IGroupService typeService) : ControllerBase
    {
        [HttpGet("by-category")]
        [ProducesResponseType(typeof(List<ProductGroupListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProductGroupListItemDto>>> GetByCategory(
            [FromQuery] int categoryId,
            [FromQuery(Name = "q")] string? search,
            CancellationToken ct = default)
        {
            if (categoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var list = await uow.ProductGroups.GetByCategoryIdAsync(
                categoryId,
                predicate: search == null ? null : pg => EF.Functions.Like(pg.Name, $"%{search}%"),
                selector: ProductGroupMappings.ToListItem,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductGroupListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductGroupListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            var list = await uow.ProductGroups.ListAsync(
                predicate: null,
                selector: ProductGroupMappings.ToListItem,
                asNoTracking: true,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetGroupById")]
        [ProducesResponseType(typeof(ProductGroupListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductGroupListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.ProductGroups.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: ProductGroupMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("گروه کالا یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ProductGroupListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductGroupListItemDto>> Create(
            [FromForm] GroupCreateForm form,
            CancellationToken ct = default)
        {
            var name = form.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (form.CategoryId <= 0)
                return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var categoryExists = await uow.Categories.AnyAsync(c => c.Id == form.CategoryId, ct);
            if (!categoryExists)
                return NotFound("دسته‌بندی یافت نشد.");

            var dupExists = await uow.ProductGroups.AnyAsync(
                g => g.CategoryId == form.CategoryId &&
                     g.Name.ToUpper() == name.ToUpperInvariant(),
                ct);
            if (dupExists)
                return Conflict("نام گروه کالا تکراری است.");

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
                    imagePath = await storage.SaveAsync(s, form.Image.FileName, "groups", ct);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var entity = new ProductGroup
            {
                Name = name,
                DisplayOrder = form.DisplayOrder,
                CategoryId = form.CategoryId,
                ImagePath = imagePath
            };

            await uow.ProductGroups.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                // Cleanup uploaded file on conflict
                if (!string.IsNullOrEmpty(imagePath))
                {
                    try { await storage.DeleteAsync(imagePath, ct); } catch { /* best-effort */ }
                }
                return Conflict("نام گروه کالا تکراری است.");
            }

            var result = ProductGroupApiMappers.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ProductGroupListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductGroupListItemDto>> UpdateWithFile(
             int id,
             [FromForm] GroupUpdateForm form,
             CancellationToken ct = default)
        {
            var entity = await uow.ProductGroups.GetByIdAsync(id, ct);

            if (entity is null)
                return NotFound("گروه کالا پیدا نشد.");

            var name = form.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (form.CategoryId <= 0)
                return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var categoryExists = await uow.Categories.AnyAsync(c => c.Id == form.CategoryId, ct);
            if (!categoryExists)
                return NotFound("دسته‌بندی یافت نشد.");

            // Only check duplicates if Category or Name changed
            var nameChanged = !string.Equals(entity.Name, name, StringComparison.Ordinal);
            var categoryChanged = entity.CategoryId != form.CategoryId;

            if (nameChanged || categoryChanged)
            {
                var dup = await uow.ProductGroups.AnyAsync(
                    g => g.Id != id
                      && g.CategoryId == form.CategoryId
                      && g.Name.ToUpper() == name.ToUpperInvariant(),
                    ct);

                if (dup)
                    return Conflict("نام گروه کالا تکراری است.");
            }

            if (form.Image is not null && form.Image.Length > 0)
            {
                // Restrict to image types here (LocalFileStorage also validates)
                var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
                var ext = Path.GetExtension(form.Image.FileName);
                if (!allowed.Contains(ext)) return BadRequest("فرمت تصویر نامعتبر است.");

                // save new
                using var s = form.Image.OpenReadStream();
                var newPath = await storage.SaveAsync(s, form.Image.FileName, "groups", ct);

                // delete old (best effort)
                try { await storage.DeleteAsync(entity.ImagePath, ct); } catch { }

                entity.ImagePath = newPath;
            }

            entity.Name = name;
            entity.DisplayOrder = form.DisplayOrder;
            entity.CategoryId = form.CategoryId;

            try
            {
                // If entity is tracked, Update() is optional:
                // uow.ProductGroups.Update(entity);
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("نام گروه کالا تکراری است.");
            }

            var result = ProductGroupApiMappers.ToListItemDto(entity);
            return Ok(result); // or NoContent() if you prefer a lighter response
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var entity = await uow.ProductGroups.GetByIdAsync(id, ct);
            if (entity is null)
                return NotFound("گروه کالا پیدا نشد.");

            try
            {
                uow.ProductGroups.Remove(entity);
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsForeignKeyViolation(ex))
            {
                return Conflict("امکان حذف گروه کالا به دلیل وابستگی وجود ندارد.");
            }

            try { await storage.DeleteAsync(entity.ImagePath, ct); } catch { }

            return NoContent(); // 204 — success, no response body
        }

        [HttpDelete("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken ct = default)
        {
            if (id <= 0) return NotFound("شناسه گروه کالا نامعتبر است.");

            var entity = await uow.ProductGroups.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("شناسه گروه کالا پیدا نشد.");

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

        #region FormRegion
        [HttpGet("by-form")]
        [ProducesResponseType(typeof(List<ProductGroupListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProductGroupListItemDto>>> GetByForm(int formId, CancellationToken ct)
        {
            if (formId <= 0) return BadRequest("شناسه فرم کالا نامعتبر است.");

            var isFormExist = await uow.Forms.AnyAsync(
                predicate: f => f.Id == formId,
                ct: ct
                );

            if (!isFormExist)
                return BadRequest("شناسه فرم کالا نامعتبر است.");

            var categoryId = await uow.Forms.GetByIdAsync(formId,
                selector: f => f.CategoryId,
                ct: ct);

            var list = await uow.ProductGroups.ListAsync(
                predicate: g => g.CategoryId == categoryId,
                selector: ProductGroupMappings.ToListItem,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct: ct
                );
                
            return Ok(list);
        }

        [HttpPost("assignments")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AddGroupToRowsDto dto, CancellationToken ct = default)
        {
            if (dto is null) return BadRequest("بدنهٔ درخواست خالی است.");
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // Basic validation
            if (dto.FormId <= 0 || dto.GroupId <= 0)
                return BadRequest("شناسه‌های فرم/نوع نامعتبر است.");

            if (dto.RowIds is null || dto.RowIds.Count == 0)
                return BadRequest("حداقل یک ردیف لازم است.");

            var rowIds = dto.RowIds.Where(id => id > 0).Distinct().ToArray();
            if (rowIds.Length == 0)
                return BadRequest("شناسهٔ ردیف‌ها نامعتبر است.");

            var existingRows = await uow.FormRows.ListAsync(
                predicate: r => r.FormId == dto.FormId && rowIds.Contains(r.Id),
                selector: r => r.Id,
                ct: ct
            );

            if (existingRows.Count != rowIds.Length)
                return BadRequest("بعضی از ردیف‌های ارسال‌شده یافت نشد یا متعلق به این فرم نیست.");

            var res = await typeService.AssignGroupToForm(
                formId: dto.FormId,
                groupId: dto.GroupId,
                rowIds: rowIds,
                displayOrder: dto.DisplayOrder,
                color: dto.Color,
                ct: ct);

            return res.Status switch
            {
                GroupStatus.FormNotFound => NotFound("شناسهٔ فرم نامعتبر است."),
                GroupStatus.DisplayOrderConflict => Conflict("ترتیب نمایش در این فرم قبلاً استفاده شده است."),
                GroupStatus.AlreadyAssigned => NoContent(),
                GroupStatus.NoContent => NoContent(),
                _ => Problem(statusCode: 500, title: "ثبت نوع برای ردیف‌ها با خطا مواجه شد.")
            };
        }
        #endregion
    }
}

