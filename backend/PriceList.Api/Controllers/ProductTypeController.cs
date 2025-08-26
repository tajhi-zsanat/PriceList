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
    public class ProductTypeController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet("by-group/{groupId:int}")]
        [ProducesResponseType(typeof(List<ProductTypeListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProductTypeListItemDto>>> GetByCategory(int groupId, CancellationToken ct)
        {
            if (groupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");

            var list = await uow.ProductTypes.GetByGroupIdAsync(
                groupId,
                selector: ProductTypeMappings.ToListItem,
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
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
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }
    }
}
