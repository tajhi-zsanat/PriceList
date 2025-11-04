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
    public sealed class TypeService(IUnitOfWork uow) : ITypeService
    {
        public async Task<AddTypeToFormResult> AssignTypeToForm(
        int formId,
        int typeId,
        IReadOnlyList<int> rowIds,
        int displayOrder,
        string? color,
        CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(TypeStatus.FormNotFound);

            await uow.BeginTransactionAsync(ct);
            try
            {
                var formTypeExists = await uow.FormTypes.AnyAsync(
                    predicate: ft => ft.FormId == formId && ft.ProductTypeId == typeId,
                    ct: ct);

                if (!formTypeExists)
                {
                    var displayOrderUsed = await uow.FormTypes.AnyAsync(
                        predicate: f => f.FormId == formId && f.DisplayOrder == displayOrder,
                        ct: ct);

                    if (displayOrderUsed)
                        return new(TypeStatus.DisplayOrderConflict);

                    await uow.ProductTypes.AddFormTypeAsync(
                        formId: formId,
                        typeId: typeId,
                        displayOrder: displayOrder,
                        color: color,
                        ct: ct);
                }

                var existingRowIds = await uow.FormRowProductTypes.ListAsync(
                    predicate: frt => frt.FormId == formId
                                      && frt.ProductTypeId == typeId
                                      && rowIds.Contains(frt.FormRowId),
                    selector: frt => frt.FormRowId,
                    ct: ct);

                var toAdd = rowIds.Except(existingRowIds).ToArray();
                if (toAdd.Length > 0)
                {
                    await uow.FormRowProductTypes.AddFormRowTypeAsync(
                        formId: formId,
                        rowIds: toAdd,
                        typeId: typeId,
                        displayOrder: displayOrder,
                        color: color,
                        ct: ct);
                }

                await uow.SaveChangesAsync(ct);
                await uow.CommitTransactionAsync(ct);

                // If there was nothing to add and formType already existed, it's still fine (idempotent)
                return toAdd.Length == 0 && formTypeExists
                    ? new(TypeStatus.AlreadyAssigned)
                    : new(TypeStatus.NoContent);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
