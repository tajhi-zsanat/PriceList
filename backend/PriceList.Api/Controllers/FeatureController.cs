using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController(IUnitOfWork uow) : ControllerBase
    {

    }
}
