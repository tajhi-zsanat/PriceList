using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Api.Dtos;
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
        [HttpGet("by-brand")]
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> GetById(int id, CancellationToken ct)
        {
            var dto = await uow.Products.FirstOrDefaultAsync(p => p.Id == id, ProductMappings.ToDetail, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> Create([FromBody] ProductCreateDto dto, CancellationToken ct)
        {
            var entity = dto.ToEntity();
            await uow.Products.AddAsync(entity, ct);
            await uow.SaveChangesAsync(ct);

            // Convert entity to detail DTO (in-memory here; you can requery with projection if needed)
            var result = new ProductDetailDto(entity.Id, entity.Model, entity.Model, entity.Description, entity.Price, entity.CreateDateAndTime);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto, CancellationToken ct)
        {
            var entity = await uow.Products.GetByIdAsync(id, ct);
            if (entity is null) return NotFound();
            entity.Apply(dto);
            uow.Products.Update(entity);
            await uow.SaveChangesAsync(ct);
            return NoContent();
        }
    }
}
