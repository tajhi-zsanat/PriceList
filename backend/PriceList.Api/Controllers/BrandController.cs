using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.Brand;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Brand;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Entities;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BrandController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<BrandListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BrandListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            var list = await uow.Brands.ListAsync(
                predicate: null,
                selector: BrandMappings.ToListItem,
                asNoTracking: true,
                orderBy: q => q
                    .OrderBy(c => c.DisplayOrder == 0)
                    .ThenBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet("by-categories")]
        [ProducesResponseType(typeof(List<BrandListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<BrandListItemDto>>> GetByCategory(
           [FromQuery] int categoryId,
           [FromQuery] int groupId,
           [FromQuery(Name = "q")] string? search,
           CancellationToken ct = default)
        {
            if (categoryId <= 0 || groupId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");

            var isCategoryExist = await uow.ProductGroups.AnyAsync(
                predicate: b => b.Id == groupId,
                ct: ct
                );

            var isGroupExist = await uow.ProductGroups.AnyAsync(
                predicate: b => b.Id == groupId,
                ct: ct
                );

            if (!isCategoryExist || !isGroupExist)
                return BadRequest("شناسه دسته بندی نامعتبر می‌باشد.");

            var list = await uow.Brands.GetBrandsAsync(categoryId, groupId, search, ct);

            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetBrandById")]
        [ProducesResponseType(typeof(BrandListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.Brands.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: BrandMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("برند یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BrandListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BrandListItemDto>> Create(
            [FromBody] BrandInputDto dto,
            CancellationToken ct = default)
        {
            var name = dto.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام برند الزامی است.");

            var dupExists = await uow.Brands.AnyAsync(
                c => c.Name.ToUpper() == name.ToUpperInvariant(),
                ct);

            if (dupExists)
                return Conflict("نام برند تکراری است.");

            var entity = new Brand
            {
                Name = name,
                DisplayOrder = dto.DisplayOrder,
                ImagePath = dto.ImagePath
            };

            await uow.Brands.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("نام دسته‌بندی تکراری است.");
            }

            var result = BrandApiMappers.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BrandListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BrandListItemDto>> Update(
            int id,
            [FromBody] BrandInputDto dto,
            CancellationToken ct = default)
        {
            var entity = await uow.Brands.GetByIdAsync(id, ct);
            if (entity is null)
                return NotFound("برند پیدا نشد.");

            var name = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام برند الزامی است.");

            // Only check duplicates if Name changed
            var nameChanged = !string.Equals(entity.Name, name, StringComparison.Ordinal);
            if (nameChanged)
            {
                var dup = await uow.Categories.AnyAsync(
                    c => c.Id != id &&
                         c.Name.ToUpper() == name.ToUpperInvariant(),
                    ct);

                if (dup)
                    return Conflict("نام برند تکراری است.");
            }

            entity.Name = name;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.ImagePath = dto.ImagePath;

            try
            {
                // If 'entity' is tracked, Update() isn't required:
                // uow.Categories.Update(entity);
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("نام دسته‌بندی تکراری است.");
            }

            var result = BrandApiMappers.ToListItemDto(entity);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            var entity = await uow.Brands.GetByIdAsync(id, ct);
            if (entity is null)
                return NotFound("برند پیدا نشد.");

            try
            {
                uow.Brands.Remove(entity);
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsForeignKeyViolation(ex))
            {
                // if you add this helper similar to IsUniqueViolation
                return Conflict("امکان حذف برند به دلیل وابستگی وجود ندارد.");
            }

            return NoContent(); // 204 — success, no response body
        }
    }
}
