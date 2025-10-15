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

        public async Task<List<FeatureNameSetGroupDto>> GroupRowsAndCellsByFeatureNamesAsync(
           int formId,
           CancellationToken ct)
        {
            // 1) Minimal projection: RowId, Cells, Feature names (distinct)
            var rows = await _db.FormRows
                .Where(r => r.FormId == formId)
                .Select(r => new
                {
                    r.Id,
                    Cells = r.Cells
                        .Select(c => new CellDto { Id = c.Id, ColIndex = c.ColIndex, Value = c.Value })
                        .ToList(),
                    // IMPORTANT: ignore values; just take feature names. Distinct in case of duplicates.
                    FeatureNames = r.Features
                        .Select(f => f.Feature.Name)
                        .Distinct()
                        .ToList()
                })
                .AsNoTracking()
                .ToListAsync(ct);

            if (rows.Count == 0) return new();

            var groups = rows
                .GroupBy(r => FeatureKeyHelper.BuildKey(r.FeatureNames))
                .Select(g =>
                {
                    // Take the ordered names from the first (all equal by key)
                    var orderedNames = g.First().FeatureNames.OrderBy(n => n, StringComparer.Ordinal).ToList();

                    var bucketRows = g.Select(x => new RowWithCellsDto
                    {
                        RowId = x.Id,
                        Cells = x.Cells.OrderBy(c => c.ColIndex).ToList()
                    }).ToList();

                    return new FeatureNameSetGroupDto
                    {
                        FeatureNames = orderedNames,   // empty list => “no features”
                        Rows = bucketRows
                    };
                })
                // Optional: put “no features” last; then sort by joined names for stability
                .OrderBy(b => b.FeatureNames.Count == 0 ? 1 : 0)
                .ThenBy(b => string.Join(",", b.FeatureNames), StringComparer.Ordinal)
                .ToList();

            return groups;
        }
    }
}
