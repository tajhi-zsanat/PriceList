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
    public class FormRowTypeRepository : GenericRepository<FormRowProductType>, IFormRowProductTypeRepo
    {
        public FormRowTypeRepository(AppDbContext db, ILogger<FormRowProductType> logger)
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
            var list = new List<FormRowProductType>(rowIds.Count);
            foreach (var rowId in rowIds)
            {
                list.Add(new FormRowProductType
                {
                    FormId = formId,
                    FormRowId = rowId,
                    ProductTypeId = typeId,
                });
            }

            await _db.AddRangeAsync(list, ct);
        }

    }
}