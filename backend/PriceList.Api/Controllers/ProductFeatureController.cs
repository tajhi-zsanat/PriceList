using Microsoft.AspNetCore.Mvc;

namespace PriceList.Api.Controllers
{
    public class ProductFeatureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
