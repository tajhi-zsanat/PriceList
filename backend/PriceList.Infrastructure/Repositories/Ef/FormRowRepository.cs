using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FormRowRepository : GenericRepository<FormRow>, IFormRowRepository
    {
        public FormRowRepository(AppDbContext db, ILogger<FormRow> logger)
       : base(db, logger)
        {
        }

        public async Task<int> GetFormByMaxRow(List<int> ids, CancellationToken ct)
        {
            var desType = await _db.FormColumnDefs
                .Where(c => c.Type == ColumnType.MultilineText && ids.Contains(c.FormId))
                .Select(c => c.Index)
                .FirstOrDefaultAsync(ct);

            var priceType = await _db.FormColumnDefs
                .Where(c => c.Type == ColumnType.Price && ids.Contains(c.FormId))
                .Select(c => c.Index)
                .FirstOrDefaultAsync(ct);

            var rowsWithDescription = _db.FormCells
                .Where(c =>
                    c.ColIndex == desType &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            var rowsWithPrice = _db.FormCells
                .Where(c =>
                    c.ColIndex == priceType &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            var validRowIds = rowsWithDescription
                .Intersect(rowsWithPrice);

            var result = await _db.FormRows
                .Where(fr => ids.Contains(fr.FormId) &&
                             validRowIds.Contains(fr.Id))
                .GroupBy(fr => fr.FormId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<int> GetCountRow(List<int> ids, CancellationToken ct)
        {
            var desType = await _db.FormColumnDefs
              .Where(c => c.Type == ColumnType.MultilineText && ids.Contains(c.FormId))
              .Select(c => c.Index)
              .FirstOrDefaultAsync(ct);

            var priceType = await _db.FormColumnDefs
                .Where(c => c.Type == ColumnType.Price && ids.Contains(c.FormId))
                .Select(c => c.Index)
                .FirstOrDefaultAsync(ct);

            var rowsWithDescription = _db.FormCells
                .Where(c =>
                    c.ColIndex == desType &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            var rowsWithPrice = _db.FormCells
                .Where(c =>
                    c.ColIndex == priceType &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

            var validRowIds = rowsWithDescription
                .Intersect(rowsWithPrice);

            var countRows = await _db.FormRows
                .Where(f => validRowIds.Contains(f.Id))
                .CountAsync(ct);

            return countRows;
        }

    }
}
