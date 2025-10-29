using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<(List<FeatureNameSetGroupDto> Groups, int TotalRows)>
         GroupRowsAndCellsByFeatureNamesPagedAsync(
            int formId,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            return (new List<FeatureNameSetGroupDto>(), 0);
            //var totalRows = await _db.FormRows
            //    .Where(r => r.FormId == formId)
            //    .CountAsync(ct);

            //if (totalRows == 0)
            //    return (new List<FeatureNameSetGroupDto>(), 0);

            //var skip = (page - 1) * pageSize;
            //var startRowNumber = skip + 1;

            //// Compute a deterministic sort key for each row:
            ////   - min positive DisplayOrder across its features
            ////   - rows with no features (or only 0/negative) go to the end (int.MaxValue)
            //var rows = await _db.FormRows
            //    .Where(r => r.FormId == formId)
            //    .Select(r => new
            //    {
            //        r.Id,
            //        SortOrder = r.Features
            //            .Select(fr => (int?)((fr.DisplayOrder.HasValue && fr.DisplayOrder > 0) ? fr.DisplayOrder : null))
            //            .Min() ?? int.MaxValue,

            //        // pick the color from the feature that defines the SortOrder, else a default
            //        FeatureColor = r.Features
            //            .OrderBy(fr => (fr.DisplayOrder.HasValue && fr.DisplayOrder > 0) ? fr.DisplayOrder : int.MaxValue)
            //            .Select(fr => fr.Color)
            //            .FirstOrDefault() ?? "#206E4E",

            //        FeatureNames = r.Features
            //            .Select(fr => fr.Feature.Name)
            //            .Distinct()
            //            .ToList(),

            //        Cells = r.Cells
            //            .Select(c => new CellDto { Id = c.Id, ColIndex = c.ColIndex, Value = c.Value })
            //            .ToList()
            //    })
            //    .OrderBy(r => r.SortOrder)
            //    .ThenBy(r => r.Id)
            //    .Skip(skip)
            //    .Take(pageSize)
            //    .AsNoTracking()
            //    .ToListAsync(ct);

            //int currentRowNumber = startRowNumber;

            //// Group but PRESERVE ordering using the carried SortOrder.
            //var groups = rows
            //    .GroupBy(r => FeatureKeyHelper.BuildKey(r.FeatureNames))
            //    .Select(g =>
            //    {
            //        // deterministic group props
            //        var orderedRows = g
            //            .OrderBy(x => x.SortOrder)
            //            .ThenBy(x => x.Id)
            //            .Select(x => new RowWithCellsDto
            //            {
            //                RowCount = currentRowNumber++,
            //                RowId = x.Id,
            //                Cells = x.Cells.OrderBy(c => c.ColIndex).ToList()
            //            })
            //            .ToList();

            //        // sort names just for display, not for ordering groups
            //        var orderedNames = g.First().FeatureNames
            //            .OrderBy(n => n, StringComparer.Ordinal)
            //            .ToList();

            //        // choose a representative color (first row’s already-sort-aligned color)
            //        var featureColor = g
            //            .OrderBy(x => x.SortOrder).ThenBy(x => x.Id)
            //            .Select(x => x.FeatureColor)
            //            .FirstOrDefault() ?? "#206E4E";

            //        // group sort key = min row SortOrder
            //        var groupSort = g.Min(x => x.SortOrder);

            //        return new
            //        {
            //            Group = new FeatureNameSetGroupDto
            //            {
            //                FeatureNames = orderedNames,
            //                FeatureColor = featureColor,
            //                Rows = orderedRows
            //            },
            //            HasNoFeatures = orderedNames.Count == 0,
            //            GroupSort = groupSort,
            //            GroupNameForTie = string.Join(",", orderedNames)
            //        };
            //    })
            //    // ✅ FINAL ordering of groups: groups with features first, then by DisplayOrder, then by name
            //    .OrderBy(x => x.HasNoFeatures ? 1 : 0)
            //    .ThenBy(x => x.GroupSort)
            //    .ThenBy(x => x.GroupNameForTie, StringComparer.Ordinal)
            //    .Select(x => x.Group)
            //    .ToList();

            //return (groups, totalRows);
        }
    }
}
