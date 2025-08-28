using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Repositories;
using PriceList.Infrastructure.Repositories.Ef;
using System.Text.RegularExpressions;

namespace PriceList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-categories")]
        [ProducesResponseType(typeof(PaginatedResult<ProductListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedResult<ProductListItemDto>>> GetByBrand(
            [FromQuery] int brandId,
            [FromQuery] int categoryId,
            [FromQuery] int groupId,
            [FromQuery] int typeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            if (brandId <= 0) return BadRequest("شناسه برند کالا نامعتبر است.");
            if (categoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");
            if (groupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");
            if (typeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");

            var result = await uow.Products.ListPagedAsync(
                predicate: p =>
                    p.BrandId == brandId &&
                    (p.CategoryId == categoryId) &&
                    (p.ProductGroupId == groupId) &&
                    (p.ProductTypeId == typeId),
                orderBy: q => q.OrderBy(p => p.Model),                  
                selector: ProductMappings.ToListItem,               
                page: page,
                pageSize: pageSize,
                ct: ct);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.Products.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: ProductMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("کالا یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductListItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductListItemDto>> Create(
            [FromBody] ProductInputDto dto,
            CancellationToken ct = default)
        {
            var model = dto.Model?.Trim();
            if (string.IsNullOrWhiteSpace(model))
                return BadRequest("نام کالا الزامی است.");

            if (dto.CategoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");
            if (dto.ProductGroupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");
            if (dto.ProductTypeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");
            if (dto.BrandId <= 0) return BadRequest("شناسه برند نامعتبر است.");
            if (dto.UnitId <= 0) return BadRequest("شناسه واحد نامعتبر است.");
            if (dto.SupplierId <= 0) return BadRequest("شناسه تامین‌کننده نامعتبر است.");

            if (!await uow.Categories.AnyAsync(c => c.Id == dto.CategoryId, ct)) return NotFound("דسته‌بندی یافت نشد.");
            if (!await uow.ProductGroups.AnyAsync(g => g.Id == dto.ProductGroupId, ct)) return NotFound("گروه کالا یافت نشد.");
            if (!await uow.ProductTypes.AnyAsync(t => t.Id == dto.ProductTypeId, ct)) return NotFound("نوع کالا یافت نشد.");
            if (!await uow.Brands.AnyAsync(b => b.Id == dto.BrandId, ct)) return NotFound("برند یافت نشد.");
            if (!await uow.Units.AnyAsync(u => u.Id == dto.UnitId, ct)) return NotFound("واحد یافت نشد.");
            if (!await uow.Suppliers.AnyAsync(s => s.Id == dto.SupplierId, ct)) return NotFound("تامین‌کننده یافت نشد.");

            // Composite duplicate check
            var normalized = model.ToUpperInvariant();
            var dupExists = await uow.Products.AnyAsync(p =>
                   p.Model != null && p.Model.ToUpper() == normalized
                && p.BrandId == dto.BrandId
                && p.ProductTypeId == dto.ProductTypeId
                && p.ProductGroupId == dto.ProductGroupId
                && p.CategoryId == dto.CategoryId
                && p.SupplierId == dto.SupplierId,
                ct);

            if (dupExists)
                return Conflict("کالایی با همین مدل، برند، نوع، گروه، دسته‌بندی و تامین‌کننده قبلاً ثبت شده است.");

            var customProps = (dto.CustomProperties ?? []).Take(3).ToList();

            var entity = new Product
            {
                Model = model,
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                Price = dto.Price,
                Number = dto.Number,
                DocumentPath = string.IsNullOrWhiteSpace(dto.DocumentPath) ? null : dto.DocumentPath,
                CategoryId = dto.CategoryId,
                ProductGroupId = dto.ProductGroupId,
                ProductTypeId = dto.ProductTypeId,
                BrandId = dto.BrandId,
                UnitId = dto.UnitId,
                SupplierId = dto.SupplierId,

                Images = (dto.ImagePaths ?? [])
                         .Where(p => !string.IsNullOrWhiteSpace(p))
                         .Select(p => new ProductImage { ImagePath = p! })
                         .ToList(),

                CustomProperties = customProps
                    .Where(cp => !string.IsNullOrWhiteSpace(cp.Key))
                    .Select(cp => new ProductCustomProperty { Key = cp.Key.Trim(), Value = cp.Value?.Trim() ?? "" })
                    .ToList()
            };

            await uow.Products.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("کالایی با همین ترکیب مشخصات از قبل وجود دارد.");
            }

            var result = ProductMappings.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }
    }
}
