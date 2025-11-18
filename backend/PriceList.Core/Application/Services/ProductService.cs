using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public sealed class ProductService(IUnitOfWork _uow) : IProductService
    {
        public async Task<FormCellsScrollResponseDto> GetProducts(
            int category,
            int group,
            int brand,
            int skip,
            int take,
            CancellationToken ct)
        {
            var formIds = await _uow.Forms.ListAsync(
                predicate: f => f.CategoryId == category
                                && f.ProductGroupId == group
                                && f.BrandId == brand,
                selector: f => f.Id,
                ct: ct);

            if (formIds == null || !formIds.Any())
            {
                return new FormCellsScrollResponseDto(
                    Status: Product.NoContent,
                    Headers: null,
                    Cells: null,
                    Meta: null);
            }

            var formId = await _uow.FormRows.GetFormByMaxRow(formIds, ct);

            if (formId == null)
            {
                return new FormCellsScrollResponseDto(
                    Status: Product.NoContent,
                    Headers: null,
                    Cells: null,
                    Meta: null);
            }

            var headers = await _uow.FormColumns.ListAsync(
                predicate: fc => fc.FormId == formId,
                selector: FormMappings.ToFormColumnDefDto,
                orderBy: q => q.OrderBy(c => c.Index),
                asNoTracking: true,
                ct: ct);

            var (groups, totalRows) = await _uow.FormFeatures
                .GroupRowsAndCellsByTypeScrollAsync(formId, skip, take, ct);

            // Count how many rows came back in this window
            var returnedRows = groups.Sum(g => g.Rows.Count);

            // 5) Build scroll meta
            var hasPrev = skip > 0;
            var hasNext = skip + returnedRows < totalRows;

            var rowCount = await _uow.FormRows.GetCountRow(formIds, ct);

            var lastUpdate = await _uow.Forms.FirstOrDefaultAsync(
                predicate: f => f.Id == formId,
                selector: f => f.UpdateDate,
                ct: ct
                );

            var meta = new ScrollMeta(
                lastUpdate,
                totalRows,
                rowCount,
                skip,
                take,
                returnedRows,
                hasPrev,
                hasNext);

            // 6) Final DTO – success
            return new FormCellsScrollResponseDto(
                Status: Product.Initial,
                Headers: headers,
                Cells: groups,
                Meta: meta);
        }
    }
}
