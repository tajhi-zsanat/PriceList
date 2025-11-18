using Azure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Application.Services;
using PriceList.Core.Enums;
using QuestPDF.Helpers;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<FormCellsScrollResponseDto>> GetAll(
            [FromQuery] int categoryId = 1,
            [FromQuery] int groupId = 10,
            [FromQuery] int brandId = 10,
            [FromQuery]  int skip = 0,
            [FromQuery]  int take = 20,
            CancellationToken ct = default)
        {
            try
            {
                var res = await _productService.GetProducts(categoryId, groupId, brandId, skip, take, ct);

                return res.Status switch
                {
                    Product.FormNotFound => NotFound("شناسهٔ فرم نامعتبر است."),
                    Product.NoContent => NoContent(),
                    Product.Initial => Ok(res),

                    // Any other status (MaxColumnsReached, AlreadyExists, etc.)
                    _ => Problem(
                        statusCode: 500,
                        title: "ثبت نوع برای ردیف‌ها با خطا مواجه شد.")
                };
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // 499 - Client Closed Request (common pattern)
                HttpContext.Response.StatusCode = 499;
                return new EmptyResult();
            }
        }
    }
}
