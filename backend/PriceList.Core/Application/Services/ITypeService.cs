using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface ITypeService
    {
        Task<AddTypeToFormResult> AssignTypeToForm(int formId,
            int typeId,
            IReadOnlyList<int> rowIds,
            int displayOrder,
            string? color,
            CancellationToken ct);
    }
}
