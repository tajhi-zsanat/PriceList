using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Api.Dtos;
using PriceList.Api.Dtos.Product;
using PriceList.Api.Dtos.Supplier;
using PriceList.Api.Helpers;
using PriceList.Api.Mappings;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Application.Dtos.Supplier;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Repositories;
using PriceList.Infrastructure.Repositories.Ef;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PriceList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IUnitOfWork uow, IFileStorage storage) : ControllerBase
    {
        /// <summary>
        /// Infinite-scroll: groups products by exact feature-set and returns a sliced page across groups.
        /// </summary>
        [HttpGet("by-categories")]
        [ProducesResponseType(typeof(ProductsWithHeaders), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductsWithHeaders>> GetByCategories(
            [FromQuery] int brandId,
            [FromQuery] int categoryId,
            [FromQuery] int groupId,
            [FromQuery] int typeId,
            // scroll parameters
            [FromQuery] int skip = 0,
            [FromQuery] int take = 20,
            // optional
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {

            // Basic normalization / guardrails
            if (skip < 0) skip = 0;
            if (take < 1) take = 20;

            // You can relax these if needed; keeping them to avoid bad queries.
            if (brandId <= 0) return BadRequest("شناسه برند کالا نامعتبر است.");
            if (categoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");
            if (groupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");
            if (typeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");

            var headers = await uow.Header.ListAsync(
                predicate: (ph => ph.BrandId == brandId && ph.ProductTypeId == typeId),
                selector: ProductHeaderMappings.ToListItem,
                asNoTracking: true,
                ct: ct
                );

            // Example: pick a default/top supplier based on your business rule.
            var supplierId = await uow.Products.GetTopSupplierAsync(ct);

            var products = await uow.Features.ListFeaturesWithProductsScrollAsync(
                skip: skip,
                take: take,
                categoryId: categoryId,
                groupId: groupId,
                typeId: typeId,
                brandId: brandId,
                supplierId: supplierId,
                productSearch: search,
                ct: ct
            );

            var result = new ProductsWithHeaders(headers, products);

            // For infinite scroll, 200 with an empty Items array is fine; don't return 404.
            return Ok(result);
        }

        [HttpGet("by-categories/suppliers-summary")]
        [ProducesResponseType(typeof(List<SupplierSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<SupplierSummaryDto>>> GetSuppliersSummary(
            [FromQuery] int brandId,
            [FromQuery] int categoryId,
            [FromQuery] int groupId,
            [FromQuery] int typeId,
            [FromQuery] int skip = 0,       // optional: for paging the supplier list itself
            [FromQuery] int take = 20,      // optional
            CancellationToken ct = default)
        {
            if (brandId <= 0) return BadRequest("شناسه برند کالا نامعتبر است.");
            if (categoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");
            if (groupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");
            if (typeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");

            // Query: group by Supplier and count
            var query = uow.Products.Query() // assume Query() exposes IQueryable<Product>
                .Where(p =>
                    p.BrandId == brandId &&
                    p.CategoryId == categoryId &&
                    p.ProductGroupId == groupId &&
                    p.ProductTypeId == typeId);

            var summaries = await query
            .GroupBy(p => new { p.SupplierId, SupplierName = p.Supplier.Name })
            .Select(g => new SupplierSummaryDto(
                g.Key.SupplierId,
                g.Key.SupplierName,
                g.Count()
            ))
            .OrderByDescending(x => x.ProductCount)
            .ThenBy(x => x.SupplierName)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);

            return Ok(summaries);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        [ProducesResponseType(typeof(ProductListItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductListItemDto>> GetById(int id, CancellationToken ct = default)
        {
            var item = (await uow.Products.FirstOrDefaultAsync(
                predicate: c => c.Id == id,
                selector: ProductMappings.ToListItem,
                ct: ct));

            return item is null ? NotFound("کالا یافت نشد.") : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductListItemDto), StatusCodes.Status201Created)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductListItemDto>> Create(
            [FromForm] ProductCreateForm form,
            CancellationToken ct = default)
        {
            var model = form.Model?.Trim();
            if (string.IsNullOrWhiteSpace(model))
                return BadRequest("نام کالا الزامی است.");

            if (form.CategoryId <= 0) return BadRequest("شناسه دسته‌بندی نامعتبر است.");
            if (form.ProductGroupId <= 0) return BadRequest("شناسه گروه کالا نامعتبر است.");
            if (form.ProductTypeId <= 0) return BadRequest("شناسه نوع کالا نامعتبر است.");
            if (form.BrandId <= 0) return BadRequest("شناسه برند نامعتبر است.");
            if (form.UnitId <= 0) return BadRequest("شناسه واحد نامعتبر است.");
            if (form.SupplierId <= 0) return BadRequest("شناسه تامین‌کننده نامعتبر است.");

            if (!await uow.Categories.AnyAsync(c => c.Id == form.CategoryId, ct)) return NotFound("דسته‌بندی یافت نشد.");
            if (!await uow.ProductGroups.AnyAsync(g => g.Id == form.ProductGroupId, ct)) return NotFound("گروه کالا یافت نشد.");
            if (!await uow.ProductTypes.AnyAsync(t => t.Id == form.ProductTypeId, ct)) return NotFound("نوع کالا یافت نشد.");
            if (!await uow.Brands.AnyAsync(b => b.Id == form.BrandId, ct)) return NotFound("برند یافت نشد.");
            if (!await uow.Units.AnyAsync(u => u.Id == form.UnitId, ct)) return NotFound("واحد یافت نشد.");
            if (!await uow.Suppliers.AnyAsync(s => s.Id == form.SupplierId, ct)) return NotFound("تامین‌کننده یافت نشد.");

            // Composite duplicate check
            var normalized = model.ToUpperInvariant();
            var dupExists = await uow.Products.AnyAsync(p =>
                   p.Model != null && p.Model.Trim().ToUpper() == normalized
                && p.BrandId == form.BrandId
                && p.ProductTypeId == form.ProductTypeId
                && p.ProductGroupId == form.ProductGroupId
                && p.CategoryId == form.CategoryId
                && p.SupplierId == form.SupplierId, ct);

            if (dupExists)
                return Conflict("کالایی با همین مدل، برند، نوع، گروه، دسته‌بندی و تامین‌کننده قبلاً ثبت شده است.");

            var entity = new Product
            {
                Model = model,
                Description = string.IsNullOrWhiteSpace(form.Description) ? null : form.Description.Trim(),
                Price = form.Price,
                Number = form.Number,
                DocumentPath = string.IsNullOrWhiteSpace(form.DocumentPath) ? null : form.DocumentPath,
                CategoryId = form.CategoryId,
                ProductGroupId = form.ProductGroupId,
                ProductTypeId = form.ProductTypeId,
                BrandId = form.BrandId,
                UnitId = form.UnitId,
                SupplierId = form.SupplierId,
            };

            await uow.Products.AddAsync(entity, ct);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (SqlExceptionHelpers.IsUniqueViolation(ex))
            {
                return Conflict("کالایی با همین ترکیب مشخصات از قبل وجود دارد.");
            }

            var imagePaths = new List<string>();

            if (form.Image is not null && form.Image.Count > 0)
            {
                var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".svg" };

                foreach (var image in form.Image)
                {
                    var ext = Path.GetExtension(image.FileName);
                    if (!allowed.Contains(ext))
                        return BadRequest("فرمت تصویر نامعتبر است.");

                    try
                    {
                        using var s = image.OpenReadStream();
                        imagePaths.Add(await storage.SaveAsync(s, image.FileName, "products", ct));
                    }
                    catch (InvalidOperationException ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }

            if (imagePaths.Count > 0)
            {
                var imgs = imagePaths.Select((p, idx) => new ProductImage
                {
                    ProductId = entity.Id,
                    ImagePath = p,
                    IsMain = idx == 0 // first image as main
                });

                await uow.ProductImageRepository.AddRangeAsync(imgs, ct);

                await uow.SaveChangesAsync(ct);
            }

            var result = ProductApiMappers.ToListItemDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, result);
        }
    }
}
