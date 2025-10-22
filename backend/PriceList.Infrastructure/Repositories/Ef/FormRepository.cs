using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
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
            await _db.SaveChangesAsync(ct); // to get Id if needed later
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
                new object[] { insertAt, utcNow, pDate, pTime, formId }, ct);
        }
    }
}
