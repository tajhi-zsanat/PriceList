using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Brand;
using PriceList.Core.Application.Mappings;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext db, ILogger<Brand> logger)
        : base(db, logger)
        {
        }

        public async Task<List<BrandListItemDto>> GetBrandsAsync(int categoryId, int groupId, string? search, CancellationToken ct = default)
        {
            var formIDs = await _db.Forms
                .Where((f => f.CategoryId == categoryId && f.ProductGroupId == groupId))
                .Select(f => f.Id)
                .ToListAsync();

            if (formIDs.Count == 0)
                return [];

            var fillFormIDs = new List<int>();

            var desType = await _db.FormColumnDefs
               .Where(c => c.Type == ColumnType.MultilineText && formIDs.Contains(c.FormId))
               .Select(c => new
               {
                   c.FormId,
                   c.Index
               })
               .ToDictionaryAsync(f => f.FormId, f => f.Index, ct);

            var priceType = await _db.FormColumnDefs
                .Where(c => c.Type == ColumnType.Price && formIDs.Contains(c.FormId))
                .Select(c => new
                {
                    c.FormId,
                    c.Index
                })
                .ToDictionaryAsync(f => f.FormId, f => f.Index, ct);

            foreach (var formId in formIDs)
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

                var rowCount = await _db.FormRows
                .Where(fr => fr.FormId == formId &&
                             validRowIds.Contains(fr.Id))
                .CountAsync(ct);

                if (rowCount > 0)
                    fillFormIDs.Add(formId);
            }

            if (fillFormIDs.Count == 0)
                return [];

            var result = await _db.Forms
                .Where(f => fillFormIDs.Contains(f.Id))
                .Select(BrandMappings.ToListItemFromForm)
                .Distinct()
                .ToListAsync();

            if(result == null)
                return [];

            return result;
        }

        public Task<Brand?> GetByNameAsync(string name, CancellationToken ct = default)
            => _db.Brands.AsNoTracking()
                  .FirstOrDefaultAsync(b => b.Name == name, ct);
    }
}
