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
    public class ProductTypeController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-group/{groupId:int}")]
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductTypeListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductTypeListItemDto>> Update(
             int id,
             [FromBody] TypeInputDto dto,
             CancellationToken ct = default)
        {
            var entity = await uow.ProductTypes.GetByIdAsync(id, ct);

            if (entity is null)
                return NotFound("نوع کالا پیدا نشد.");

            var name = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام نوع کالا الزامی است.");

            if (dto.GroupId <= 0)
                return BadRequest("شناسه گروه کالا نامعتبر است.");

            var categoryExists = await uow.Categories.AnyAsync(c => c.Id == dto.GroupId, ct);
            if (!categoryExists)
                return NotFound("گروه کالا یافت نشد.");

            // Only check duplicates if Category or Name changed
            var nameChanged = !string.Equals(entity.Name, name, StringComparison.Ordinal);
            var groupChanged = entity.ProductGroupId != dto.GroupId;

            if (nameChanged || groupChanged)
            {
                var dup = await uow.ProductGroups.AnyAsync(
                    g => g.Id != id
                      && g.CategoryId == dto.GroupId
                      && g.Name.ToUpper() == name.ToUpperInvariant(),
                    ct);

                if (dup)
                    return Conflict("نام نوع کالا تکراری است.");
            }

            entity.Name = name;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.ImagePath = dto.ImagePath;
            entity.ProductGroupId = dto.GroupId;

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("نام گروه کالا تکراری است.");
            }

            var result = ProductTypeMappings.ToListItemDto(entity);
            return Ok(result); 
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductTypeListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductTypeListItemDto>> Create(
          [FromBody] TypeInputDto dto,
          CancellationToken ct = default)
        {
            var name = dto.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("نام گروه کالا الزامی است.");

            if (dto.GroupId <= 0)
                return BadRequest("شناسه گروه کالا نامعتبر است.");

            var groupExists = await uow.ProductGroups.AnyAsync(c => c.Id == dto.GroupId, ct);
            if (!groupExists)
                return NotFound("گروه کالا یافت نشد.");

            var dupExists = await uow.ProductGroups.AnyAsync(
                g => g.CategoryId == dto.GroupId &&
                     g.Name.ToUpper() == name.ToUpperInvariant(),
                ct);

            if (dupExists)
                return Conflict("نام گروه کالا تکراری است.");

            var entity = new ProductType
            {
                Name = name,
                DisplayOrder = dto.DisplayOrder,
                ProductGroupId = dto.GroupId,
                ImagePath = dto.ImagePath
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

            var result = ProductTypeMappings.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
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
    }
}
