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
    public class CategoryController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryListItemDto>>> GetAll(CancellationToken ct)
        {
            var list = await uow.Categories.ListAsync(
                predicate: null
                , selector: CategoryMappings.ToListItem
                , orderBy: q => q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
                , ct);

            return Ok(list);
        }
    }
}
