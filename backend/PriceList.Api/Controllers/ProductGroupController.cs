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
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
                ct);

            return Ok(list); // 200 with [] if none
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductGroupListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductGroupListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            var list = await uow.ProductGroups.ListAsync(
                predicate: null,
                selector: ProductGroupMappings.ToListItem,
                orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name),
                ct);

            return Ok(list);
        }
    }
}

