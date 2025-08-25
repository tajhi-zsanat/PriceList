using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Api.Dtos;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Repositories;

namespace PriceList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductListItemDto>>> GetAll(CancellationToken ct)
        {
            var list = await uow.Products.ListAsync(predicate: null, selector: ProductMappings.ToListItem, ct);
            return Ok(list);
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
