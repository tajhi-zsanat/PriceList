using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormRepository : IGenericRepository<Form>
    {
        Task<List<FormListItemDto>> GetFormsAsync(string userId, CancellationToken ct);
        Task<Form> GetFormByIdAsync(int formId, CancellationToken ct);
        Task<DateTime> GetLastFormOrViewUpdatedAsync(CancellationToken ct);

        Task<bool> FormExistsAsync(int formId, CancellationToken ct);
        Task<(bool hasC1, bool hasC2, bool hasC3, int posUnit, int posC1, int posC2, int colCount)>
            GetColumnLayoutMetaAsync(int formId, CancellationToken ct);

        Task ShiftColumnsAsync(int formId, int insertAt, CancellationToken ct);
        Task<int> InsertColumnAsync(int formId, int insertAt, ColumnType type, string title, CancellationToken ct);

        Task ShiftCellsAsync(int formId, int insertAt, CancellationToken ct);
        Task InsertCellsForColumnAsync(int formId, int insertAt, CancellationToken ct);

        Task DoUpdateDateAndTimeAsync(int formId, CancellationToken ct);

        //Delete Section
        Task RemoveColumnDefByIndexAsync(int formId, int index, CancellationToken ct);
        /// <summary>
        /// Reindexes dynamic defs (startIndex inclusive; typically 6) and
        /// returns a stable mapping { oldIndex -> newIndex }.
        /// Also updates Type/Key/Title for custom columns.
        /// </summary>
        Task<Dictionary<int, int>> ReindexColumnDefsAsync(int formId, int startIndex, CancellationToken ct);

        Task DeleteCellsByColumnIndexAsync(int formId, int index, CancellationToken ct);

        /// <summary>
        /// Applies a {old -> new} index mapping to all cells of the form in one SQL update.
        /// </summary>
        Task ApplyCellIndexMappingAsync(int formId, IReadOnlyDictionary<int, int> map, CancellationToken ct);


        // Add Feature
        Task<int> CreateFeatureAsync(
            int formId,
            string feature,
            int displayOrder,
            string color,
            CancellationToken ct);

        Task<int> UpdateFormRowsAsync(int formId, int featureId, int[] rowIDs, CancellationToken ct);
        Task<int> DeleteFormRowsAsync(int featureId, CancellationToken ct);
        Task<int> AddFormRowsAsync(int[] rowIds, int featureId, CancellationToken ct);

        Task<bool> AllRowsAlreadyHaveFeatureAsync(
             int formId, int featureId, int[] rowIds, CancellationToken ct);

        //Task<AddRowToForm> AddRow(
        //      int formId, int featureId, CancellationToken ct);

        Task ShiftRowsAsync(int formId, int insertAt, CancellationToken ct);

        Task<int> CreateRowAsync(int formId, int featureId, int insertAt, CancellationToken ct);

        Task<bool> UpdateFormAsync(int formId, StatusForm status, CancellationToken ct);

        Task CreateCellsAsync(int formId, int rowId, CancellationToken ct);

        Task<bool> FeatureExistsAsync(int featureId, CancellationToken ct);
        Task<bool> RowIndexExistsAsync(int formId, int rowId, CancellationToken ct);

        Task ShiftMinusRowsAsync(int formId, int removeAt, CancellationToken ct);

        Task<List<GetRowNumberList>> GetRowNumberAsync(int formId, CancellationToken ct);
    }
}
