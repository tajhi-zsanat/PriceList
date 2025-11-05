using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Application.Dtos.ProductFeature;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FormCellRepository : GenericRepository<FormCell>, IFormCellRepository
    {
        public FormCellRepository(AppDbContext db, ILogger<FormCell> logger)
       : base(db, logger)
        {
        }

        //public async Task<(List<GridGroupByType> Groups, int TotalRows)>
        //    GroupRowsAndCellsByTypePagedAsync(
        //        int formId,
        //        int page,
        //        int pageSize,
        //        CancellationToken ct)
        //{
        //    // --- 1) Total rows for pagination
        //    var totalRows = await _db.FormRows
        //        .AsNoTracking()
        //        .Where(r => r.FormId == formId)
        //        .CountAsync(ct);

        //    if (totalRows == 0)
        //        return (new List<GridGroupByType>(), 0);

        //    // --- 2) Page math and stable row ordering
        //    var skip = (page - 1) * pageSize;
        //    var startRowNumber = skip + 1;

        //    var pageRowIds = await _db.FormRows
        //        .AsNoTracking()
        //        .Where(r => r.FormId == formId)
        //        .OrderBy(r => r.RowIndex)    
        //        .ThenBy(r => r.Id)
        //        .Skip(skip)
        //        .Take(pageSize)
        //        .Select(r => r.Id)
        //        .ToListAsync(ct);

        //    // Short-circuit (unlikely because totalRows > 0)
        //    if (pageRowIds.Count == 0)
        //        return (new List<GridGroupByType>(), totalRows);

        //    // --- 3) Load cells for rows in page (batched)
        //    var cellsByRow = await _db.FormCells
        //        .AsNoTracking()
        //        .Where(c => pageRowIds.Contains(c.RowId))
        //        .Select(c => new
        //        {
        //            c.Id,
        //            c.RowId,
        //            c.ColIndex,
        //            c.Value
        //        })
        //        .ToListAsync(ct);

        //    var rowCellsMap = cellsByRow
        //        .GroupBy(x => x.RowId)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Select(c => new CellDto
        //            {
        //                Id = c.Id,
        //                ColIndex = c.ColIndex,
        //                Value = c.Value
        //            }).ToList()
        //        );

        //    // --- 4) Load all product types configured for this form (ordered)
        //    var formTypes = await _db.FormProductGroups
        //        .AsNoTracking()
        //        .Where(ft => ft.FormId == formId)
        //        .OrderBy(ft => ft.DisplayOrder)
        //        .Select(ft => new
        //        {
        //            ft.ProductGroupId,
        //            TypeName = ft.ProductGroup.Name,
        //            ft.Color
        //        })
        //        .ToListAsync(ct);

        //    var typeIdsForForm = formTypes.Select(t => t.ProductGroupId).ToHashSet();

        //    // --- 5) Load which page rows have which types (batched)
        //    var pageRowTypes = await _db.FormRowProductGroups
        //        .AsNoTracking()
        //        .Where(frt => frt.FormId == formId && pageRowIds.Contains(frt.FormRowId))
        //        .Select(frt => new { frt.FormRowId, frt.ProductGroupId })
        //        .ToListAsync(ct);

        //    // Map: typeId -> HashSet<rowId> (only for rows in this page)
        //    var rowsByType = pageRowTypes
        //        .GroupBy(x => x.ProductGroupId)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => g.Select(x => x.FormRowId).ToHashSet()
        //        );

        //    // --- 6) Build groups in memory
        //    var groups = new List<GridGroupByType>();
        //    var assignedRowIds = new HashSet<int>();

        //    int currentRowNumber = startRowNumber;

        //    // 6a) For each configured type on the form (keep order by DisplayOrder)
        //    foreach (var t in formTypes)
        //    {
        //        if (!rowsByType.TryGetValue(t.ProductGroupId, out var rowSet) || rowSet.Count == 0)
        //            continue; // No rows of this type in the current page

        //        var rows = pageRowIds
        //            .Where(id => rowSet.Contains(id)) // keep the page order
        //            .Select(id => new RowWithCellsDto
        //            {
        //                RowId = id,
        //                RowCount = currentRowNumber++,
        //                Cells = rowCellsMap.TryGetValue(id, out var cells) ? cells : new List<CellDto>()
        //            })
        //            .ToList();

        //        if (rows.Count > 0)
        //        {
        //            groups.Add(new GridGroupByType
        //            {
        //                TypeName = t.TypeName,
        //                typeId = t.ProductGroupId,
        //                Color = t.Color,
        //                Rows = rows
        //            });

        //            foreach (var id in rows.Select(r => r.RowId))
        //                assignedRowIds.Add(id);
        //        }
        //    }

        //    // 6b) Rows with NO type (within this page)
        //    var withoutTypeIds = pageRowIds.Where(id => !assignedRowIds.Contains(id)).ToList();
        //    if (withoutTypeIds.Count > 0)
        //    {
        //        var rows = withoutTypeIds
        //            .Select(id => new RowWithCellsDto
        //            {
        //                RowId = id,
        //                RowCount = currentRowNumber++,
        //                Cells = rowCellsMap.TryGetValue(id, out var cells) ? cells : new List<CellDto>()
        //            })
        //            .ToList();

        //        groups.Add(new GridGroupByType
        //        {
        //            TypeName = "بدون دسته‌بندی",
        //            typeId = -1,
        //            Color = "#1d6646",
        //            Rows = rows
        //        });
        //    }

        //    return (groups, totalRows);
        //}
    }
}
