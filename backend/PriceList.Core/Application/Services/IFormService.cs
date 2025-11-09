using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface IFormService
    {
        Task<AddColDefResult> AddCustomColDef(string CustomCol, int FormId, CancellationToken ct);

        Task<RemoveColDefResult> RemoveCustomColDef(int Index, int FormId, CancellationToken ct);

        Task<AddFeatureToFormResult> AddFeature(
            int formId,
            string feature,
            int[] rowIds,
            int displayOrder,
            string color,
            CancellationToken ct);

        Task<AddRowToFormResult> AddRowAndCell(
            int formId,
            int featureId,
            int rowIndex,
            CancellationToken ct);
    }
}
