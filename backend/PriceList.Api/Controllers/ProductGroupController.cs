using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductGroupController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-category/{categoryId:int}")]
        [ProducesResponseType(typeof(List<ProductGroupListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProductGroupListItemDto>>> GetByCategory(
            [FromRoute] int categoryId,
            CancellationToken ct = default)
        {
            if (categoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var list = await uow.ProductGroups.GetByCategoryIdAsync(
                categoryId,
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
        [ProducesResponseType(typeof(ProductGroupListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductGroupListItemDto>> Create(
          [FromBody] GroupInputDto dto,
          CancellationToken ct = default)
        {
            var name = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (dto.CategoryId <= 0)
                return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var categoryExists = await uow.Categories.AnyAsync(c => c.Id == dto.CategoryId, ct);
            if (!categoryExists)
                return NotFound("دسته‌بندی یافت نشد.");

            var dupExists = await uow.ProductGroups.AnyAsync(
                g => g.CategoryId == dto.CategoryId &&
                     g.Name.ToUpper() == name.ToUpperInvariant(),
                ct);

            if (dupExists)
                return Conflict("نام گروه کالا تکراری است.");

            var entity = new ProductGroup
            {
                Name = name,
                DisplayOrder = dto.DisplayOrder,
                CategoryId = dto.CategoryId,
                ImagePath = dto.ImagePath
            };

            await uow.ProductGroups.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                // Covers race conditions when a unique index exists
                return Conflict("نام گروه کالا تکراری است.");
            }

            var result = ProductGroupMappings.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductGroupListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductGroupListItemDto>> Update(
             int id,
             [FromBody] GroupInputDto dto,
             CancellationToken ct = default)
        {
            var entity = await uow.ProductGroups.GetByIdAsync(id, ct);

            if (entity is null)
                return NotFound("گروه کالا پیدا نشد.");

            var name = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (dto.CategoryId <= 0)
                return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var categoryExists = await uow.Categories.AnyAsync(c => c.Id == dto.CategoryId, ct);
            if (!categoryExists)
                return NotFound("دسته‌بندی یافت نشد.");

            // Only check duplicates if Category or Name changed
            var nameChanged = !string.Equals(entity.Name, name, StringComparison.Ordinal);
            var categoryChanged = entity.CategoryId != dto.CategoryId;

            if (nameChanged || categoryChanged)
            {
                var dup = await uow.ProductGroups.AnyAsync(
                    g => g.Id != id
                      && g.CategoryId == dto.CategoryId
                      && g.Name.ToUpper() == name.ToUpperInvariant(),
                    ct);

                if (dup)
                    return Conflict("نام گروه کالا تکراری است.");
            }

            entity.Name = name;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.ImagePath = dto.ImagePath;
            entity.CategoryId = dto.CategoryId;

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

            var result = ProductGroupMappings.ToListItemDto(entity);
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
                // if you add this helper similar to IsUniqueViolation
                return Conflict("امکان حذف گروه کالا به دلیل وابستگی وجود ندارد.");
            }

            return NoContent(); // 204 — success, no response body
        }
    }
}

