using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class FormViewRepository : GenericRepository<FormView>, IFormViewRepository
    {
        public FormViewRepository(AppDbContext db, ILogger<FormView> logger)
        : base(db, logger)
        {
        }

        public Task<bool> HasUserViewedFormAsync(
     int formId,
     string viewerKey,
     CancellationToken ct = default)
        {
            return _db.FormViews.AnyAsync(v =>
                v.FormId == formId &&
                v.ViewerKey == viewerKey, ct);
        }

        public async Task<List<PopularFormDto>> GetTopPopularFormsAsync(int topCount, CancellationToken ct = default)
        {
            // Using FromSqlInterpolated to pass parameter safely
            var result = await _db.PopularForms
                .FromSqlInterpolated($"EXEC dbo.GetTopPopularForms @TopCount={topCount}")
                .AsNoTracking()
                .ToListAsync(ct);

            return result;
        }
    }
}
