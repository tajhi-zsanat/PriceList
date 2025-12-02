using Azure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Application.Services;
using PriceList.Core.Entities;
using PriceList.Core.Enums;
using QuestPDF.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _productService, IFormViewService _formViewService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<FormCellsScrollResponseDto>> GetAll(
            int categoryId = 1,
            int groupId = 10,
            int brandId = 10,
            int skip = 0,
            int take = 20,
            int? formId = null,
            CancellationToken ct = default)
        {
            try
            {
                var res = await _productService.GetProducts(categoryId, groupId, brandId, skip, take, formId, ct);

                if (res.Status == Product.Initial)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    string viewerKey;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        viewerKey = $"user:{userId}";
                    }
                    else
                    {
                        const string cookieName = "pl_viewer_id";

                        if (!Request.Cookies.TryGetValue(cookieName, out var anonId) || string.IsNullOrEmpty(anonId))
                        {
                            anonId = Guid.NewGuid().ToString("N");

                            Response.Cookies.Append(cookieName, anonId, new CookieOptions
                            {
                                HttpOnly = true,
                                IsEssential = true,
                                Expires = DateTimeOffset.UtcNow.AddYears(1),
                                SameSite = SameSiteMode.None,
                                Secure = true
                            });
                        }

                        viewerKey = $"anon:{anonId}";
                    }

                    var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                    var ua = HttpContext.Request.Headers["User-Agent"].ToString();

                    await _formViewService.RegisterViewAsync(res.formId.Value, viewerKey, ip, ua, ct);
                }

                return res.Status switch
                {
                    Product.FormNotFound => NotFound("شناسهٔ فرم نامعتبر است."),
                    Product.NoContent => NoContent(),
                    Product.Initial => Ok(res),

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

        [HttpGet("popular")]
        public async Task<ActionResult<PopularFormDto>> GetPopularProducts(CancellationToken ct = default)
        {
            try
            {
                var popular = await _formViewService.GetTopPopularForms(4, ct);
                return Ok(popular);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = 499;
                return new EmptyResult();
            }
        }
    }
}
