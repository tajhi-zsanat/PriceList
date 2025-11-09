using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using PriceList.Infrastructure.Data.Migrations;
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
        int page,
        int pageSize,
        CancellationToken ct)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // 1) Count all rows
            var totalRows = await _db.FormRows
                .AsNoTracking()
                .Where(r => r.FormId == formId)
                .CountAsync(ct);

            if (totalRows == 0)
                return (new List<GridGroupByFeature>(), 0);

            var skip = (page - 1) * pageSize;

            // 2) Build the ordered, paged list of rows with their feature metadata.
            //    Left-join to features; null-feature gets DisplayOrder = int.MaxValue to push it last.
            var baseQuery =
                from r in _db.FormRows.AsNoTracking().Where(r => r.FormId == formId)
                join fraw in _db.FormFeature.AsNoTracking().Where(f => f.FormId == formId)
                    on r.FormFeatureId equals (int?)fraw.Id into gj
                from f in gj.DefaultIfEmpty()
                select new
                {
                    r.Id,
                    r.RowIndex,
                    FeatureId = r.FormFeatureId,                  // nullable int
                    FeatureName = f != null ? f.Name : "بدون ویژگی",
                    FeatureColor = f != null ? f.Color : "#206e4e",
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
    }
}
