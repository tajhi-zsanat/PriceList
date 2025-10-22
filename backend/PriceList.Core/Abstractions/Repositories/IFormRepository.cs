using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormRepository : IGenericRepository<Form>
    {
        Task<bool> FormExistsAsync(int formId, CancellationToken ct);
        Task<(bool hasC1, bool hasC2, bool hasC3, int posUnit, int posC1, int posC2, int colCount)>
            GetColumnLayoutMetaAsync(int formId, CancellationToken ct);

        Task ShiftColumnsAsync(int formId, int insertAt, CancellationToken ct);
        Task<int> InsertColumnAsync(int formId, int insertAt, ColumnType type, string title, CancellationToken ct);

        Task ShiftCellsAsync(int formId, int insertAt, CancellationToken ct);
        Task InsertCellsForColumnAsync(int formId, int insertAt, CancellationToken ct);
    }
}
