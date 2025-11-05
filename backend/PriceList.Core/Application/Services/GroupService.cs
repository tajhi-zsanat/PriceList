using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public sealed class GroupService(IUnitOfWork uow) : IGroupService
    {
        public async Task<AddGroupToFormResult> AssignGroupToForm(
        int formId,
        int groupId,
        IReadOnlyList<int> rowIds,
        int displayOrder,
        string? color,
        CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(GroupStatus.FormNotFound);

            await uow.BeginTransactionAsync(ct);
            try
            {
                var formTypeExists = await uow.FormGroups.AnyAsync(
                    predicate: ft => ft.FormId == formId && ft.ProductGroupId == groupId,
                    ct: ct);

                if (!formTypeExists)
                {
                    var displayOrderUsed = await uow.FormGroups.AnyAsync(
                        predicate: f => f.FormId == formId && f.DisplayOrder == displayOrder,
                        ct: ct);

                    if (displayOrderUsed)
                        return new(GroupStatus.DisplayOrderConflict);

                    await uow.ProductTypes.AddFormTypeAsync(
                        formId: formId,
                        typeId: groupId,
                        displayOrder: displayOrder,
                        color: color,
                        ct: ct);
                }

                var existingRowIds = await uow.FormRowProductGroups.ListAsync(
                    predicate: frt => frt.FormId == formId
                                      && frt.ProductGroupId == groupId
                                      && rowIds.Contains(frt.FormRowId),
                    selector: frt => frt.FormRowId,
                    ct: ct);

                var toAdd = rowIds.Except(existingRowIds).ToArray();
                if (toAdd.Length > 0)
                {
                    await uow.FormRowProductGroups.AddFormRowTypeAsync(
                        formId: formId,
                        rowIds: toAdd,
                        typeId: groupId,
                        displayOrder: displayOrder,
                        color: color,
                        ct: ct);
                }

                await uow.SaveChangesAsync(ct);
                await uow.CommitTransactionAsync(ct);

                // If there was nothing to add and formType already existed, it's still fine (idempotent)
                return toAdd.Length == 0 && formTypeExists
                    ? new(GroupStatus.AlreadyAssigned)
                    : new(GroupStatus.NoContent);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
