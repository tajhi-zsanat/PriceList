using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Entities;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryListItemDto>>> GetAll(CancellationToken ct)
        {
            var list = await uow.Categories.ListAsync(
                predicate: null
                , selector: CategoryMappings.ToListItem
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
        public async Task<ActionResult<CategoryListItemDto>> Create(
            [FromForm] CategoryCreateForm form,
            [FromServices] IFileStorage storage,
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

            var result = CategoryMappings.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CategoryListItemDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryListItemDto>> UpdateWithFile(
            int id,
            [FromForm] CategoryUpdateForm form,
            [FromServices] IFileStorage storage,
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

            // optional image replace
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
            return Ok(CategoryMappings.ToListItemDto(entity));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            [FromServices] IFileStorage storage,
            CancellationToken ct = default)
        {
            var entity = await uow.Categories.GetByIdAsync(id, ct);
            if (entity is null) return NotFound("دسته‌بندی پیدا نشد.");

            // remove DB row
            uow.Categories.Remove(entity);
            await uow.SaveChangesAsync(ct);

            // best-effort file delete (don’t fail the request if this throws)
            try { await storage.DeleteAsync(entity.ImagePath, ct); } catch { /* log if needed */ }

            return NoContent();
        }
    }
}
