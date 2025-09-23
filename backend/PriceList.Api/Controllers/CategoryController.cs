using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.Category;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Category;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Entities;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController(IUnitOfWork uow, IFileStorage storage) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryListItemDto>>> GetAll(CancellationToken ct)
        {
            var list = await uow.Categories.ListAsync(
                predicate: null
                , selector: CategoryMappings.ToListItem,
                asNoTracking: true
                , orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                , ct);

            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        [ProducesResponseType(typeof(CategoryListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.Categories.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: CategoryMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("دسته بندی یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CategoryListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CategoryListItemDto>> Create(
            [FromForm] CategoryCreateForm form,
            CancellationToken ct = default)
        {
            var name = form.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام دسته‌بندی الزامی است.");

            var dupExists = await uow.Categories.AnyAsync(
                c => c.Name.ToUpper() == name.ToUpperInvariant(), ct);
            if (dupExists)
                return Conflict("نام دسته‌بندی تکراری است.");

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
                    imagePath = await storage.SaveAsync(s, form.Image.FileName, "categories", ct);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var entity = new Category
            {
                Name = name,
                DisplayOrder = form.DisplayOrder,
                ImagePath = imagePath
            };

            await uow.Categories.AddAsync(entity, ct);
            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                if (!string.IsNullOrEmpty(imagePath))
                    await storage.DeleteAsync(imagePath, ct);

                return Conflict("نام دسته‌بندی تکراری است.");
            }

            var result = CategoryApiMappers.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CategoryListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryListItemDto>> UpdateWithFile(
            int id,
            [FromForm] CategoryUpdateForm form,
            CancellationToken ct = default)
        {
            var entity = await uow.Categories.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("דسته‌بندی پیدا نشد.");

            var name = form.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام دسته‌بندی الزامی است.");

            // only if name changed
            if (!string.Equals(entity.Name, name, StringComparison.Ordinal))
            {
                var dup = await uow.Categories.AnyAsync(
                    c => c.Id != id && c.Name.ToUpper() == name.ToUpperInvariant(), ct);
                if (dup) return Conflict("نام دسته‌بندی تکراری است.");
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
                var newPath = await storage.SaveAsync(s, form.Image.FileName, "categories", ct);

                // delete old (best effort)
                try { await storage.DeleteAsync(entity.ImagePath, ct); } catch { }

                entity.ImagePath = newPath;
            }

            entity.Name = name;
            entity.DisplayOrder = form.DisplayOrder;

            await uow.SaveChangesAsync(ct);
            return Ok(CategoryApiMappers.ToListItemDto(entity));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct = default)
        {
            var entity = await uow.Categories.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("دسته‌بندی پیدا نشد.");

            // remove DB row
            uow.Categories.Remove(entity);
            await uow.SaveChangesAsync(ct);

            // best-effort file delete (don’t fail the request if this throws)
            try { await storage.DeleteAsync(entity.ImagePath, ct); } catch { }

            return NoContent();
        }

        [HttpDelete("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(int id, CancellationToken ct = default)
        {
            if (id <= 0) return NotFound("شناسه دسته‌بندی نامعتبر است.");

            var entity = await uow.Categories.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("دسته‌بندی پیدا نشد.");

            // If there’s no image, treat as idempotent delete.
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
