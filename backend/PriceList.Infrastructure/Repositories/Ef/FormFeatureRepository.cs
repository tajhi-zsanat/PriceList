using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using PriceList.Infrastructure.Data.Migrations;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FormFeatureRepository : GenericRepository<FormFeature>, IFormFeatureRepository
    {
        public FormFeatureRepository(AppDbContext db, ILogger<FormFeature> logger)
        : base(db, logger)
        {
        }

        public async Task<(List<GridGroupByFeature> Groups, int TotalRows)>
      GroupRowsAndCellsByTypePagedAsync(
        int formId,
        int userId,
        int page,
        int pageSize,
        CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // 1) Count all rows
            var totalRows = await _db.FormRows
                .AsNoTracking()
                .Where(r => r.FormId == formId && r.Form.UserId == userId)
                .CountAsync(ct);

            if (totalRows == 0)
                return (new List<GridGroupByFeature>(), 0);

            var skip = (page - 1) * pageSize;

            // 2) Build the ordered, paged list of rows with their feature metadata.
            //    Left-join to features; null-feature gets DisplayOrder = int.MaxValue to push it last.
            var baseQuery =
                from r in _db.FormRows.AsNoTracking().Where(r => r.FormId == formId && r.Form.UserId == userId)
                join fraw in _db.FormFeature.AsNoTracking().Where(f => f.FormId == formId)
                    on r.FormFeatureId equals (int?)fraw.Id into gj
                from f in gj.DefaultIfEmpty()
                select new
                {
                    r.Id,
                    r.RowIndex,
                    FeatureId = r.FormFeatureId,                  // nullable int
                    FeatureName = f != null ? f.Name : "بدون ویژگی",
                    FeatureColor = f != null ? f.Color : "#767676",
                    FeatureOrder = f != null ? f.DisplayOrder : int.MaxValue
                };

            var pageRows = await baseQuery
                .OrderBy(x => x.FeatureOrder)
                .ThenBy(x => x.FeatureName)    // tie-breaker for stability
                .ThenBy(x => x.RowIndex)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);

            if (pageRows.Count == 0)
                return (new List<GridGroupByFeature>(), totalRows);

            // 3) Fetch the cells for just these rows (1 extra round-trip).
            var rowIds = pageRows.Select(x => x.Id).ToList();

            var cells = await _db.FormCells.AsNoTracking()
                .Where(c => rowIds.Contains(c.RowId))
                .OrderBy(c => c.ColIndex)
                .Select(c => new
                {
                    c.RowId,
                    Cell = new CellDto
                    {
                        Id = c.Id,
                        ColIndex = c.ColIndex,
                        Value = c.Value
                    }
                })
                .ToListAsync(ct);

            var cellsByRow = cells
                .GroupBy(x => x.RowId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Cell).ToList());

            // 4) Materialize groups
            var groups = pageRows
                .GroupBy(r => new
                {
                    FeatureId = r.FeatureId ?? 0,       // 0 => "no feature" for DTO
                    r.FeatureName,
                    r.FeatureColor,
                    r.FeatureOrder
                })
                .OrderBy(g => g.Key.FeatureOrder)       // null-feature (int.MaxValue) last
                .ThenBy(g => g.Key.FeatureName)
                .Select(g => new GridGroupByFeature
                {
                    FeatureId = g.Key.FeatureId,
                    FeatureName = g.Key.FeatureName,
                    Color = g.Key.FeatureColor,
                    Rows = g.OrderBy(r => r.RowIndex).ThenBy(r => r.Id)
                            .Select(r => new RowWithCellsDto
                            {
                                RowId = r.Id,
                                RowIndex = r.RowIndex,
                                RowCount = 0, // set in step 5
                                Cells = cellsByRow.TryGetValue(r.Id, out var list) ? list : new List<CellDto>()
                            })
                            .ToList()
                })
                .ToList();

            // 5) Continuous RowCount across the *page*
            var n = skip;
            foreach (var g in groups)
                foreach (var row in g.Rows)
                    row.RowCount = ++n;

            return (groups, totalRows);
        }

        public async Task<(IReadOnlyList<GridGroupByFeature> Groups, int TotalRows)>
    GroupRowsAndCellsByTypeScrollAsync(
        int formId,
        int skip,
        int take,
        CancellationToken ct)
        {
            if (take < 1) take = 20;
            if (skip < 0) skip = 0;

            // 0) Find column "indexes" (or Ids) for description and price
            var desColId = await _db.FormColumnDefs
                .Where(c => c.FormId == formId && c.Type == ColumnType.MultilineText)
                .Select(c => c.Index) // or .Select(c => c.Index) if ColIndex uses Index
                .FirstOrDefaultAsync(ct);

            var priceColId = await _db.FormColumnDefs
                .Where(c => c.FormId == formId && c.Type == ColumnType.Price)
                .Select(c => c.Index) // or .Select(c => c.Index)
                .FirstOrDefaultAsync(ct);

            // If one of the columns does not exist, there are no "valid" rows
            if (desColId == 0 || priceColId == 0)
                return (new List<GridGroupByFeature>(), 0);

            // 1) Build query for rows that have a non-empty DESCRIPTION cell
            var rowsWithDescription = _db.FormCells
                .Where(c =>
                    c.ColIndex == desColId &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            // 2) Build query for rows that have a non-empty PRICE cell
            var rowsWithPrice = _db.FormCells
                .Where(c =>
                    c.ColIndex == priceColId &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            // 3) Only rows that have BOTH description and price
            var validRowIds = rowsWithDescription.Intersect(rowsWithPrice);

            // 4) Count only valid rows in this form
            var totalRows = await _db.FormRows
                .AsNoTracking()
                .Where(r => r.FormId == formId && validRowIds.Contains(r.Id))
                .CountAsync(ct);

            if (totalRows == 0)
                return (new List<GridGroupByFeature>(), 0);

            // 5) Base query: only valid rows
            var baseQuery =
                from r in _db.FormRows.AsNoTracking()
                    .Where(r => r.FormId == formId && validRowIds.Contains(r.Id))
                join fraw in _db.FormFeature.AsNoTracking().Where(f => f.FormId == formId)
                    on r.FormFeatureId equals (int?)fraw.Id into gj
                from f in gj.DefaultIfEmpty()
                select new
                {
                    r.Id,
                    r.RowIndex,
                    FeatureId = r.FormFeatureId,
                    FeatureName = f != null ? f.Name : "بدون ویژگی",
                    FeatureColor = f != null ? f.Color : "#767676",
                    FeatureOrder = f != null ? f.DisplayOrder : int.MaxValue
                };

            // 6) Load current scroll window (only valid rows)
            var pageRows = await baseQuery
                .OrderBy(x => x.FeatureOrder)
                .ThenBy(x => x.FeatureName)
                .ThenBy(x => x.RowIndex)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);

            if (pageRows.Count == 0)
                return (new List<GridGroupByFeature>(), totalRows);

            // 7) Load cells only for these rowIds
            var rowIds = pageRows.Select(x => x.Id).ToList();

            var cells = await _db.FormCells.AsNoTracking()
                .Where(c => rowIds.Contains(c.RowId))
                .OrderBy(c => c.ColIndex)
                .Select(c => new
                {
                    c.RowId,
                    Cell = new CellDto
                    {
                        Id = c.Id,
                        ColIndex = c.ColIndex,
                        Value = c.Value
                    }
                })
                .ToListAsync(ct);

            var cellsByRow = cells
                .GroupBy(x => x.RowId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Cell).ToList());

            // 8) Group rows by feature
            var groups = pageRows
                .GroupBy(r => new
                {
                    FeatureId = r.FeatureId ?? 0,
                    r.FeatureName,
                    r.FeatureColor,
                    r.FeatureOrder
                })
                .OrderBy(g => g.Key.FeatureOrder)
                .ThenBy(g => g.Key.FeatureName)
                .Select(g => new GridGroupByFeature
                {
                    FeatureId = g.Key.FeatureId,
                    FeatureName = g.Key.FeatureName,
                    Color = g.Key.FeatureColor,
                    Rows = g.OrderBy(r => r.RowIndex).ThenBy(r => r.Id)
                        .Select(r => new RowWithCellsDto
                        {
                            RowId = r.Id,
                            RowIndex = r.RowIndex,
                            RowCount = 0, // filled below
                            Cells = cellsByRow.TryGetValue(r.Id, out var list)
                                ? list
                                : new List<CellDto>()
                        })
                        .ToList()
                })
                .ToList();

            // 9) Set RowCount as a running counter starting from skip
            var n = skip;
            foreach (var g in groups)
                foreach (var row in g.Rows)
                    row.RowCount = ++n;

            return (groups, totalRows);
        }
    }
}
