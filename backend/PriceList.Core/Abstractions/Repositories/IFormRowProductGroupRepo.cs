using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormRowProductGroupRepo : IGenericRepository<FormRowProductGroup>
    {
        Task AddFormRowTypeAsync(int formId,
            IReadOnlyList<int> rowIds,
            int typeId,
            int displayOrder,
            string? color,
            CancellationToken ct = default);
    }
}
