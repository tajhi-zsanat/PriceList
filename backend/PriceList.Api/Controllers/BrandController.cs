using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Api.Dtos;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BrandController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-type/{typeId:int}")]
        [ProducesResponseType(typeof(List<BrandListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<BrandListItemDto>>> GetByCategory(int typeId, CancellationToken ct)
        {
            if (typeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");

            var list = await uow.Brands.ByProductTypeAsync(
                typeId,
                selector: BrandMappings.ToListItem,
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BrandListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BrandListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            var list = await uow.Brands.ListAsync(
                predicate: null,
                selector: BrandMappings.ToListItem,
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }
    }
}
