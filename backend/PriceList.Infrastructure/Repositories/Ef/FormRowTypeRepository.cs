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
    public class FormRowTypeRepository : GenericRepository<FormRowProductGroup>, IFormRowProductGroupRepo
    {
        public FormRowTypeRepository(AppDbContext db, ILogger<FormRowProductGroup> logger)
       : base(db, logger)
        {
        }

        public async Task AddFormRowTypeAsync(
             int formId,
             IReadOnlyList<int> rowIds,
             int typeId,
             int displayOrder,
             string? color,
             CancellationToken ct = default)
        {
            var list = new List<FormRowProductGroup>(rowIds.Count);
            foreach (var rowId in rowIds)
            {
                list.Add(new FormRowProductGroup
                {
                    FormId = formId,
                    FormRowId = rowId,
                    ProductGroupId = typeId,
                });
            }

            await _db.AddRangeAsync(list, ct);
        }

    }
}