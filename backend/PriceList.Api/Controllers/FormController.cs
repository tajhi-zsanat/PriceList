using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceList.Api.Dtos.Header;
using PriceList.Api.Dtos.ProductFeature;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.ProductGroup;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using System;
using System.Text.RegularExpressions;

namespace PriceList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController(IUnitOfWork uow) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<FormListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FormListItemDto>>> GetAll(
            CancellationToken ct = default)
        {
            try
            {
                var list = await uow.Form.ListAsync(
                    predicate: (f => f.SupplierId == 1),
                    selector: FormMappings.ToListItem,
                    asNoTracking: true,
                    orderBy: q => q
                        .OrderBy(c => c.DisplayOrder == 0)
                        .ThenBy(c => c.FormTitle),
                    ct);

                return Ok(list);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                HttpContext.Response.StatusCode = 499; 
                return new EmptyResult();
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] FormCreateDto dto, CancellationToken ct = default)
        {
            if (dto is null)
                return BadRequest("بدنه درخواست خالی است.");


            if (dto.BrandId <= 0 || dto.CategoryId <= 0 || dto.GroupId <= 0 || dto.TypeId <= 0)
                return BadRequest("شناسه نامعتبر می‌باشد.");

            if (dto.ColumnCount < 6 || dto.ColumnCount > 9)
                return BadRequest("حداقل تعداد ستون برای جدول 6 و حداکثر 9 می باشد.");

            var brand = await uow.Brands.GetByIdAsync(dto.BrandId, ct);
            var category = await uow.Categories.GetByIdAsync(dto.CategoryId, ct);
            var group = await uow.ProductGroups.GetByIdAsync(dto.GroupId, ct);
            var type = await uow.ProductTypes.GetByIdAsync(dto.TypeId, ct);

            if (brand is null) return NotFound("برند یافت نشد.");
            if (category is null) return NotFound("دسته‌بندی یافت نشد.");
            if (group is null) return NotFound("گروه محصول یافت نشد.");
            if (type is null) return NotFound("نوع محصول یافت نشد.");

            var formExists = await uow.Form.AnyAsync(
                f => (f.BrandId == dto.BrandId &&
                f.CategoryId == dto.CategoryId &&
                f.ProductGroupId == dto.GroupId &&
                f.ProductTypeId == dto.TypeId)
                );

            if (formExists)
                return BadRequest("فرم قبلا! ایجاد شده است.");

            if (!string.IsNullOrEmpty(dto.FormTitle))
            {
                var existingTitle = await uow.Form.AnyAsync(f => f.FormTitle == dto.FormTitle && f.SupplierId == 1);

                if (existingTitle)
                    return BadRequest("عنوان فرم تکراری می باشد.");
            }

            var entity = new Form
            {
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                ProductTypeId = dto.TypeId,
                ProductGroupId = dto.GroupId,
                SupplierId = 1,
                FormTitle = dto.FormTitle,
                ColumnCount = dto.ColumnCount,
                RowCount = dto.RowCount,
                DisplayOrder = dto.DisplayOrder
            };

            await uow.Form.AddAsync(entity);

            try
            {
                await uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                return Conflict("مقادیر ارسال‌شده با یک فرم موجود تداخل دارد.");
            }

            return NoContent();
        }
    }
}
