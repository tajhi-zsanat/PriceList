using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public interface IGroupService
    {
        Task<AddGroupToFormResult> AssignGroupToForm(int formId,
            int groupId,
            IReadOnlyList<int> rowIds,
            int displayOrder,
            string? color,
            CancellationToken ct);
    }
}
