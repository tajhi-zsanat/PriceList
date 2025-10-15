﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using PriceList.Api.Dtos.Form;
using PriceList.Api.Dtos.Header;
using PriceList.Api.Dtos.ProductFeature;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Abstractions.Storage;
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
    public class FormController(IUnitOfWork uow, IFileStorage storage) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<FormListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FormListItemDto>>> GetAll(
            int formId,
           CancellationToken ct = default)
        {
            try
            {
                var list = await uow.Forms.ListAsync(
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

        //[HttpGet("{formId:int}/meta")]
        //public async Task<ActionResult<FormColumnDefDto>> GetMeta(int formId, CancellationToken ct)
        //{
        //    var form = await uow.Form.GetByIdAsync(formId, ct);
        //    if (form == null) return NotFound();

        //    var item = new FormMetaDto(form.Id
        //        , form.FormTitle
        //        , form.Rows
        //        , form.Columns.OrderBy(c => c.Index).Select(FormMappings.ToColumnDto).ToList()
        //    );

        //    return Ok(item);
        //}

        [HttpGet("{formId:int}/allCells")]
        public async Task<ActionResult<List<FormCellsListItemDto>>> GetCells(
            int formId, CancellationToken ct = default)
        {
            try
            {
                var list = await uow.FormCells.GroupRowsAndCellsByFeatureNamesAsync(formId, ct);

                var headers = await uow.FormColumns.ListAsync(
                    predicate: (fc => fc.FormId == formId),
                    selector: FormMappings.ToFormColumnDefDto,
                    orderBy: (c => c.OrderBy(c => c.Index)),
                    asNoTracking: true,
                    ct: ct
                    );

                var res = new FormCellsResponseDto(headers, list);

                return Ok(res);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Request was cancelled by the client → return 499 or just end silently
                HttpContext.Response.StatusCode = 499; // Client Closed Request (non-standard)
                return new EmptyResult();
            }
        }

        [HttpPut("cell")]
        [ProducesResponseType(typeof(FormCellDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FormCellDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FormCellDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FormCellDto>> UpsertCell(
            [FromBody] InsertCellDto dto,
            CancellationToken ct = default)
        {
            if (dto is null)
                return BadRequest("بدنه درخواست خالی است.");

            // Load tracked entity (NO projection)
            var existing = await uow.FormCells.FirstOrDefaultAsync<FormCell>(
                predicate: c => c.Id == dto.Id,
                selector: c => c,          
                asNoTracking: false,
                ct: ct);

            if (existing == null)
                return BadRequest("سلول یافت نشد.");

            // UPDATE
            existing.Value = dto.Value;

            uow.FormCells.Update(existing);
            await uow.SaveChangesAsync(ct);

            return StatusCode(StatusCodes.Status201Created);
        }

        // 4) Upload image/file for a cell
        [HttpPut("UploadImage")]
        [RequestSizeLimit(10_000_000)]
        public async Task<ActionResult> UploadMedia([FromForm] UploadImageDto dto, CancellationToken ct)
        {
            if (dto is null || dto.File is null)
                return BadRequest("لطفاً عکس را وارد نمایید.");

            var existing = await uow.FormCells.FirstOrDefaultAsync<FormCell>(
               predicate: c => c.Id == dto.Id,
               selector: c => c,
               asNoTracking: false,
               ct: ct);

            if (existing == null)
                return BadRequest("سلول یافت نشد.");

            // Restrict to image types here (LocalFileStorage also validates)
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
            var ext = Path.GetExtension(dto.File.FileName);
            if (!allowed.Contains(ext)) return BadRequest("فرمت تصویر نامعتبر است.");

            // save new
            using var s = dto.File.OpenReadStream();
            var newPath = await storage.SaveAsync(s, dto.File.FileName, "products", ct);

            // delete old (best effort)
            try { await storage.DeleteAsync(existing.Value, ct); } catch { }

            existing.Value = newPath;


            uow.FormCells.Update(existing);
            await uow.SaveChangesAsync(ct);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(FormMetaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] FormCreateDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto is null) return BadRequest("بدنه درخواست خالی است.");

            // Validate columns and rows
            if (dto.Columns is < FormBuilder.MinTotalCols or > FormBuilder.MaxTotalCols)
                return BadRequest($"تعداد ستون باید بین {FormBuilder.MinTotalCols} و {FormBuilder.MaxTotalCols} باشد.");

            if (dto.Rows < 1)
                return BadRequest("تعداد سطر باید حداقل ۱ باشد.");

            // Validate ids
            if (dto.BrandId <= 0 || dto.CategoryId <= 0 || dto.GroupId <= 0 || dto.TypeId <= 0)
                return BadRequest("شناسه نامعتبر می‌باشد.");

            // Load lookups
            var brand = await uow.Brands.GetByIdAsync(dto.BrandId, ct);
            var category = await uow.Categories.GetByIdAsync(dto.CategoryId, ct);
            var group = await uow.ProductGroups.GetByIdAsync(dto.GroupId, ct);
            var type = await uow.ProductTypes.GetByIdAsync(dto.TypeId, ct);

            if (brand is null) return NotFound("برند یافت نشد.");
            if (category is null) return NotFound("دسته‌بندی یافت نشد.");
            if (group is null) return NotFound("گروه محصول یافت نشد.");
            if (type is null) return NotFound("نوع محصول یافت نشد.");

            // Resolve supplier from auth/tenant (TODO)
            const int supplierId = 1;

            // Duplicate check (composite)
            var existsCombo = await uow.Forms.AnyAsync(f =>
                f.SupplierId == supplierId &&
                f.BrandId == dto.BrandId &&
                f.CategoryId == dto.CategoryId &&
                f.ProductGroupId == dto.GroupId &&
                f.ProductTypeId == dto.TypeId, ct);

            if (existsCombo)
                return Conflict("فرم با این ترکیب قبلاً ایجاد شده است.");

            // Duplicate title (optional)
            if (!string.IsNullOrWhiteSpace(dto.FormTitle))
            {
                var title = dto.FormTitle.Trim();
                var existsTitle = await uow.Forms.AnyAsync(
                    f => f.SupplierId == supplierId && f.FormTitle == title, ct);
                if (existsTitle)
                    return Conflict("عنوان فرم تکراری می‌باشد.");
            }

            // Create entity
            var entity = new Form
            {
                SupplierId = supplierId,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                ProductGroupId = dto.GroupId,
                ProductTypeId = dto.TypeId,
                FormTitle = string.IsNullOrWhiteSpace(dto.FormTitle) ? null : dto.FormTitle.Trim(),
                Rows = dto.Rows,
                DisplayOrder = dto.DisplayOrder
            };

            await uow.BeginTransactionAsync(ct);
            try
            {
                await uow.Forms.AddAsync(entity, ct);
                await uow.SaveChangesAsync(ct); // get entity.Id

                var cols = FormBuilder.BuildDefaultColumns(entity.Id, dto.Columns);
                await uow.FormColumns.AddRangeAsync(cols, ct);
                await uow.SaveChangesAsync(ct); // ensure Index & ids are persisted

                var formRows = FormBuilder.BuildDefaultFormRows(entity.Id, dto.Rows);
                await uow.FormRows.AddRangeAsync(formRows, ct);
                await uow.SaveChangesAsync(ct);

                var cells = FormBuilder.BuildDefaultCells(entity.Id, cols, formRows);
                await uow.FormCells.AddRangeAsync(cells, ct);
                await uow.SaveChangesAsync(ct);

                await uow.CommitTransactionAsync(ct);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (DbUpdateException)
            {
                await uow.RollbackTransactionAsync(ct);
                return Conflict("مقادیر ارسال‌شده با یک فرم موجود تداخل دارد.");
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw; // or: return Problem("خطای غیرمنتظره رخ داد.");
            }
        }

    }
}
