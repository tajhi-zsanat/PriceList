using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Api.Dtos.ProductFeature;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Header;
using PriceList.Core.Application.Mappings;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FeatureController(IUnitOfWork uow) : ControllerBase
    {
       
    }
}
