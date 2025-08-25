using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Api.Dtos;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryListItemDto>>> GetAll(CancellationToken ct)
        {
            var list = await uow.Categories.ListAsync(
                predicate: null
                , selector: CategoryMappings.ToListItem
                , ct);
            return Ok(list);
        }
    }
}
