using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Common;
using PriceList.Core.Entities;
using PriceList.Core.Enums;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FormRepository : GenericRepository<Form>, IFormRepository
    {

        public FormRepository(AppDbContext db, ILogger<Form> logger)
        : base(db, logger)
        {
        }

        // -----------------------------
        // 🔹 1) Helper method (private)
        // -----------------------------
        private static (DateTime utcNow, string pDate, string pTime) GetAudit()
        {
            var utcNow = DateTime.UtcNow;

            var iranTz = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");

            var local = TimeZoneInfo.ConvertTimeFromUtc(utcNow, iranTz);
            var pc = new PersianCalendar();
            var pDate = $"{pc.GetYear(local):0000}/{pc.GetMonth(local):00}/{pc.GetDayOfMonth(local):00}";
            var pTime = local.ToString("HHmm");

            return (utcNow, pDate, pTime);
        }

        public async Task<bool> FormExistsAsync(int formId, CancellationToken ct)
           => await _db.Forms.AnyAsync(f => f.Id == formId, ct);

        public async Task<(bool hasC1, bool hasC2, bool hasC3, int posUnit, int posC1, int posC2, int colCount)>
        GetColumnLayoutMetaAsync(int formId, CancellationToken ct)
        {
            var cols = await _db.FormColumnDefs
                .Where(c => c.FormId == formId)
                .Select(c => new { c.Index, c.Type })
                .OrderBy(c => c.Index)
                .ToListAsync(ct);

            bool hasC1 = false, hasC2 = false, hasC3 = false;
            int posUnit = -1, posC1 = -1, posC2 = -1;

            for (int i = 0; i < cols.Count; i++)
            {
                var t = cols[i].Type;
                if (t == ColumnType.Custom1) { hasC1 = true; posC1 = i; }
                else if (t == ColumnType.Custom2) { hasC2 = true; posC2 = i; }
                else if (t == ColumnType.Custom3) { hasC3 = true; }
                else if (t == ColumnType.Select && posUnit == -1) { posUnit = i; }
            }

            return (hasC1, hasC2, hasC3, posUnit, posC1, posC2, cols.Count);
        }

        public async Task ShiftColumnsAsync(int formId, int insertAt, CancellationToken ct)
        {
            var (utcNow, pDate, pTime) = GetAudit();

            await _db.FormColumnDefs
                .Where(c => c.FormId == formId && c.Index >= insertAt)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.Index, c => c.Index + 1)
                    .SetProperty(c => c.UpdateDateAndTime, _ => utcNow)
                    .SetProperty(c => c.UpdateDate, _ => pDate)
                    .SetProperty(c => c.UpdateTime, _ => pTime), ct);
        }

        public async Task<int> InsertColumnAsync(int formId, int insertAt, ColumnType type, string title, CancellationToken ct)
        {
            var col = new FormColumnDef
            {
                FormId = formId,
                Index = insertAt,
                Kind = ColumnKind.Dynamic,
                Type = type,
                Key = type.ToString(),
                Title = title
            };
            await _db.FormColumnDefs.AddAsync(col, ct);
            await _db.SaveChangesAsync(ct);
            return col.Id;
        }

        public async Task ShiftCellsAsync(int formId, int insertAt, CancellationToken ct)
        {
            var (utcNow, pDate, pTime) = GetAudit();
            var rowIds = _db.FormRows.Where(r => r.FormId == formId).Select(r => r.Id);

            await _db.FormCells
                .Where(cell => rowIds.Contains(cell.RowId) && cell.ColIndex >= insertAt)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(c => c.ColIndex, c => c.ColIndex + 1)
                    .SetProperty(c => c.UpdateDateAndTime, _ => utcNow)
                    .SetProperty(c => c.UpdateDate, _ => pDate)
                    .SetProperty(c => c.UpdateTime, _ => pTime), ct);
        }

        public async Task InsertCellsForColumnAsync(int formId, int insertAt, CancellationToken ct)
        {
            var (utcNow, pDate, pTime) = GetAudit();

            var sql = """
        INSERT INTO FormCells
            (RowId, ColIndex, Value,
             CreateDateAndTime, UpdateDateAndTime,
             CreateDate, CreateTime, UpdateDate, UpdateTime)
        SELECT r.Id, @p0, NULL,
               @p1, @p1,
               @p2, @p3, @p2, @p3
        FROM FormRows AS r
        WHERE r.FormId = @p4;
     """;

            await _db.Database.ExecuteSqlRawAsync(
                sql,
                new object[] { insertAt, utcNow, pDate, pTime, formId },
                ct);
        }

        //Delete Section

        public async Task RemoveColumnDefByIndexAsync(int formId, int index, CancellationToken ct)
        {
            var def = await _db.FormColumnDefs
                .FirstOrDefaultAsync(c => c.FormId == formId && c.Index == index, ct);

            if (def is null) return;

            _db.FormColumnDefs.Remove(def);
            await _db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Reindex dynamic part of defs and return {old -> new}. startIndex is usually 6.
        /// </summary>
        public async Task<Dictionary<int, int>> ReindexColumnDefsAsync(
            int formId,
            int startIndex,
            CancellationToken ct)
        {
            var defs = await _db.FormColumnDefs
                .Where(c => c.FormId == formId && c.Index >= startIndex)
                .OrderBy(c => c.Index)
                .ToListAsync(ct);

            var map = new Dictionary<int, int>(defs.Count);
            var next = startIndex;

            foreach (var d in defs)
            {
                map[d.Index] = next;
                d.Index = next;

                // If this is one of the custom columns, recompute based on new slot
                if (d.Type == ColumnType.Custom1 ||
                    d.Type == ColumnType.Custom2 ||
                    d.Type == ColumnType.Custom3)
                {
                    var newType = FormBuilder.TakeColumnType(next);
                    d.Type = newType;
                    d.Key = newType.ToString();
                    //d.Title = string.IsNullOrEmpty(d.Title) ? FormBuilder.TitleColumn(next) : d.Title;
                }

                next++;
            }

            await _db.SaveChangesAsync(ct);
            return map;
        }

        public async Task ApplyCellIndexMappingAsync(
            int formId,
            IReadOnlyDictionary<int, int> map,
            CancellationToken ct)
        {
            if (map is null || map.Count == 0) return;

            // Build CASE expression and IN list with parameters
            var olds = map.Keys.ToArray();
            var news = olds.Select(o => map[o]).ToArray();

            var sbCase = new StringBuilder("CASE c.ColIndex ");
            for (int i = 0; i < olds.Length; i++)
                sbCase.Append($"WHEN @o{i} THEN @n{i} ");
            sbCase.Append("ELSE c.ColIndex END");

            var inList = string.Join(",", Enumerable.Range(0, olds.Length).Select(i => $"@o{i}"));

            var sql = $@"
UPDATE c
SET ColIndex = {sbCase}
FROM FormCells AS c
INNER JOIN FormRows AS r ON r.Id = c.RowId
WHERE r.FormId = @formId
  AND c.ColIndex IN ({inList});";

            var parameters = new List<object> { new SqlParameter("@formId", formId) };
            for (int i = 0; i < olds.Length; i++)
            {
                parameters.Add(new SqlParameter($"@o{i}", olds[i]));
                parameters.Add(new SqlParameter($"@n{i}", news[i]));
            }

            await _db.Database.ExecuteSqlRawAsync(sql, parameters.ToArray(), ct);
        }

        public async Task DeleteCellsByColumnIndexAsync(int formId, int index, CancellationToken ct)
        {
            // Fast set-based delete with join to rows of the form
            var sql = @"
DELETE c
FROM FormCells AS c
INNER JOIN FormRows AS r ON r.Id = c.RowId
WHERE r.FormId = @formId AND c.ColIndex = @index;";

            var p = new[]
            {
        new SqlParameter("@formId", formId),
        new SqlParameter("@index", index),
    };

            await _db.Database.ExecuteSqlRawAsync(sql, p, ct);
        }
    }
}
