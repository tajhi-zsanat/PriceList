using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Entities;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Services
{
    public sealed class FormService(IUnitOfWork uow) : IFormService
    {
        private const int MaxColumns = 11;
        //public async Task<AddColDefResult> AddCustomColDef(
        //    string CustomCol,
        //    int FormId,
        //    CancellationToken ct)
        //{
        //    const int OFFSET = 1000;

        //    var formExists = await uow.Forms.AnyAsync(f => f.Id == FormId, ct);
        //    if (!formExists) return new(AddColDefStatus.FormNotFound);

        //    var cols = await uow.FormColumns.ListAsync(
        //        predicate: c => c.FormId == FormId,
        //        selector: c => c,
        //        asNoTracking: false,
        //        ct: ct);

        //    if (cols.Count >= MaxColumns)
        //        return new(AddColDefStatus.MaxColumnsReached);

        //    var customTypes = new[] { ColumnType.Custom1, ColumnType.Custom2, ColumnType.Custom3 };
        //    var existingCustoms = cols.Where(c => customTypes.Contains(c.Type)).Select(c => c.Type).ToHashSet();

        //    if (existingCustoms.Contains(ColumnType.Custom3))
        //        return new(AddColDefStatus.AlreadyExists);

        //    var title = CustomCol;
        //    if (string.IsNullOrWhiteSpace(title)) title = "عنوان ستون";

        //    await uow.BeginTransactionAsync(ct);
        //    try
        //    {
        //        var newCol = new FormColumnDef
        //        {
        //            FormId = FormId,
        //            Index = int.MaxValue,
        //            Kind = ColumnKind.Dynamic,
        //            Type = ColumnType.NotAssign,
        //            Key = ColumnType.NotAssign.ToString(),
        //            Title = title
        //        };
        //        await uow.FormColumns.AddAsync(newCol, ct);
        //        await uow.SaveChangesAsync(ct);

        //        cols = await uow.FormColumns.ListAsync(
        //            predicate: c => c.FormId == FormId,
        //            selector: c => c,
        //            orderBy: c => c.OrderBy(c => c.Index),
        //            asNoTracking: false,
        //            ct: ct);

        //        var lastIndex = cols.Count - 1;
        //        var newColInList = cols[lastIndex];



        //        int posCustom1 = cols.FindIndex(c => c.Type == ColumnType.Custom1);
        //        int posCustom2 = cols.FindIndex(c => c.Type == ColumnType.Custom2);
        //        int posCustom3 = cols.FindIndex(c => c.Type == ColumnType.Custom3);

        //        int posUnit = cols.FindIndex(c => c.Type == ColumnType.Select);

        //        int insertAt;

        //        if (!existingCustoms.Contains(ColumnType.Custom1))
        //        {
        //            newColInList.Type = ColumnType.Custom1;
        //            newColInList.Key = nameof(ColumnType.Custom1);
        //            insertAt = Math.Max(0, posUnit) + 1;
        //        }
        //        else if (!existingCustoms.Contains(ColumnType.Custom2))
        //        {
        //            newColInList.Type = ColumnType.Custom2;
        //            newColInList.Key = nameof(ColumnType.Custom2);
        //            insertAt = posCustom1 + 1;
        //        }
        //        else
        //        {
        //            newColInList.Type = ColumnType.Custom3;
        //            newColInList.Key = nameof(ColumnType.Custom3);
        //            insertAt = posCustom2 + 1;
        //        }
        //        cols.RemoveAt(lastIndex);
        //        cols.Insert(insertAt, newColInList);

        //        var oldToNew = new Dictionary<int, int>(cols.Count);
        //        for (int i = 0; i < cols.Count; i++)
        //        {
        //            oldToNew[cols[i].Index] = i;
        //        }

        //        foreach (var c in cols)
        //        {
        //            var final = oldToNew[c.Index];
        //            c.Index = OFFSET + final;
        //            uow.FormColumns.Update(c);
        //        }
        //        await uow.SaveChangesAsync(ct);

        //        var allCells = await uow.FormCells.ListAsync(
        //            predicate: cell => cell.Row.FormId == FormId,
        //            selector: cell => cell,
        //            asNoTracking: false,
        //            ct: ct);

        //        foreach (var cell in allCells)
        //        {
        //            if (oldToNew.TryGetValue(cell.ColIndex, out var newIdx))
        //            {
        //                cell.ColIndex = OFFSET + newIdx;
        //                uow.FormCells.Update(cell);
        //            }
        //        }
        //        await uow.SaveChangesAsync(ct);

        //        foreach (var c in cols)
        //        {
        //            c.Index -= OFFSET;
        //            uow.FormColumns.Update(c);
        //        }
        //        foreach (var cell in allCells)
        //        {
        //            cell.ColIndex -= OFFSET;
        //            uow.FormCells.Update(cell);
        //        }
        //        await uow.SaveChangesAsync(ct);

        //        var finalNewColIndex = cols.First(x => x.Id == newCol.Id).Index;

        //        var rowIds = await uow.FormRows.ListAsync(
        //            predicate: r => r.FormId == FormId,
        //            selector: r => r.Id,
        //            asNoTracking: true,
        //            ct: ct);

        //        foreach (var rowId in rowIds)
        //        {
        //            var cell = new FormCell
        //            {
        //                RowId = rowId,
        //                ColIndex = finalNewColIndex,
        //                Value = null
        //            };
        //            await uow.FormCells.AddAsync(cell, ct);
        //        }
        //        await uow.SaveChangesAsync(ct);

        //        await uow.CommitTransactionAsync(ct);
        //        return new(AddColDefStatus.Created, NewColumnIndex: finalNewColIndex);
        //    }
        //    catch
        //    {
        //        await uow.RollbackTransactionAsync(ct);
        //        throw;
        //    }
        //}

        public async Task<AddColDefResult> AddCustomColDef(string title, int formId, CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(MappingColDefStatus.FormNotFound);

            var (hasC1, hasC2, hasC3, posUnit, posC1, posC2, colCount) =
                await uow.Forms.GetColumnLayoutMetaAsync(formId, ct);

            if (colCount >= MaxColumns) return new(MappingColDefStatus.MaxColumnsReached);
            if (hasC3) return new(MappingColDefStatus.AlreadyExists);

            var finalTitle = string.IsNullOrWhiteSpace(title) ? "عنوان سرگروه" : title;

            ColumnType newType;
            int insertAt;
            if (!hasC1) { newType = ColumnType.Custom1; insertAt = (posUnit >= 0 ? posUnit : -1) + 1; }
            else if (!hasC2) { newType = ColumnType.Custom2; insertAt = (posC1 >= 0 ? posC1 : colCount - 1) + 1; }
            else { newType = ColumnType.Custom3; insertAt = (posC2 >= 0 ? posC2 : colCount - 1) + 1; }
            if (insertAt < 0) insertAt = 0;

            await uow.BeginTransactionAsync(ct);
            try
            {
                await uow.Forms.ShiftColumnsAsync(formId, insertAt, ct);
                var _ = await uow.Forms.InsertColumnAsync(formId, insertAt, newType, finalTitle, ct);
                await uow.Forms.ShiftCellsAsync(formId, insertAt, ct);
                await uow.Forms.InsertCellsForColumnAsync(formId, insertAt, ct);

                await uow.CommitTransactionAsync(ct);
                return new(MappingColDefStatus.Created, NewColumnIndex: insertAt);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<RemoveColDefResult> RemoveCustomColDef(int formId, int index, CancellationToken ct)
        {
            const int DynamicStart = 6;

            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(RemoveColDefStatus.FormNotFound);

            if (index < DynamicStart)
                return new(RemoveColDefStatus.NoContent);

            await uow.BeginTransactionAsync(ct);
            try
            {
                // 1) Remove the column definition
                await uow.Forms.RemoveColumnDefByIndexAsync(formId, index, ct);

                // 2) Remove ALL cells that belong to that deleted column index
                await uow.Forms.DeleteCellsByColumnIndexAsync(formId, index, ct);

                // 3) Build old->new mapping from remaining defs (dynamic part starts at 6)
                var map = await uow.Forms.ReindexColumnDefsAsync(formId, DynamicStart, ct);

                // 4) Apply mapping atomically (SQL UPDATE ... CASE)
                await uow.Forms.ApplyCellIndexMappingAsync(formId, map, ct);

                await uow.CommitTransactionAsync(ct);
                return new(RemoveColDefStatus.NoContent);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}
