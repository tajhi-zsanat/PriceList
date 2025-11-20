using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
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
            if (ids == null || ids.Count == 0)
                return 0;

            var mostRows = new Dictionary<int, int>();

            var desType = await _db.FormColumnDefs
               .Where(c => c.Type == ColumnType.MultilineText && ids.Contains(c.FormId))
               .Select(c => new
               {
                   c.FormId,
                   c.Index
               })
               .ToDictionaryAsync(f => f.FormId, f => f.Index, ct);

            var priceType = await _db.FormColumnDefs
                .Where(c => c.Type == ColumnType.Price && ids.Contains(c.FormId))
                .Select(c => new
                {
                    c.FormId,
                    c.Index
                })
                .ToDictionaryAsync(f => f.FormId, f => f.Index, ct);

            foreach (var formId in ids)
            {
                if (!desType.TryGetValue(formId, out int type))
                    continue;

                if (!priceType.TryGetValue(formId, out int price))
                    continue;

                var rowsWithDescription = _db.FormCells
                .Where(c =>
                    c.Row.FormId == formId &&
                    c.ColIndex == type &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

                var rowsWithPrice = _db.FormCells
                .Where(c =>
                    c.Row.FormId == formId &&
                    c.ColIndex == price &&
                    c.Value != null &&
                    c.Value != "")
                .Select(c => c.RowId);

                var validRowIds = rowsWithDescription
                 .Intersect(rowsWithPrice);

                var result = await _db.FormRows
                .Where(fr => fr.FormId == formId &&
                             validRowIds.Contains(fr.Id))
                .CountAsync(ct);

                mostRows[formId] = result;
            }

            if (mostRows.Count == 0)
                return 0;

            int keyOfMax = mostRows.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return keyOfMax;
        }

        public async Task<int> GetCountRow(List<int> ids, CancellationToken ct)
        {
            var desType = await _db.FormColumnDefs
              .Where(c =>
              ids.Contains(c.FormId) &&
              c.Type == ColumnType.MultilineText
              && ids.Contains(c.FormId))
              .Select(c => c.Index)
              .FirstOrDefaultAsync(ct);

            var priceType = await _db.FormColumnDefs
                .Where(c =>
                ids.Contains(c.FormId) &&
                c.Type == ColumnType.Price &&
                ids.Contains(c.FormId))
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
