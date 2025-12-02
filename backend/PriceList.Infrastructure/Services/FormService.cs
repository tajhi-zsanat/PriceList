using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Form;
using PriceList.Core.Application.Services;
using PriceList.Core.Entities;
using PriceList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Services
{
    public sealed class FormService(IUnitOfWork uow) : IFormService
    {
        private const int MaxColumns = 11;

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

        public async Task<AddEditFeatureToFormResult> AddFeature(
                int formId, string feature, int[] rowIds, int displayOrder, string? color, CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(FeatureStatus.FormNotFound, null);

            await uow.BeginTransactionAsync(ct);
            try
            {
                var IsExistFeature = await uow.FormFeatures.AnyAsync(
                    predicate: f => f.Name.Trim() == feature.Trim(),
                    ct: ct
                    );

                if (IsExistFeature)
                    return new(FeatureStatus.IsExistFeature, null);

                var IsDisplayOrderExist = await uow.FormFeatures.AnyAsync(
                    predicate: f => (f.DisplayOrder == displayOrder &&
                    f.FormId == formId),
                     ct: ct
                    );

                if (IsDisplayOrderExist)
                    return new(FeatureStatus.DisplayOrderConflict, null);

                var featureId = await uow.Forms.CreateFeatureAsync(formId, feature, displayOrder, color, ct);

                if (await uow.Forms.AllRowsAlreadyHaveFeatureAsync(formId, featureId, rowIds, ct))
                {
                    await uow.RollbackTransactionAsync(ct); // nothing to do
                    return new(FeatureStatus.AlreadyAssigned, null);
                }

                var affected = await uow.Forms.UpdateFormRowsAsync(formId, featureId, rowIds, ct);

                var featureIDs = await uow.FormFeatures.ListAsync(
                    predicate: fr => fr.FormId == formId,
                    selector: fr => fr.Id,
                    ct: ct);

                var formRowFeatureIDs = await uow.FormRows.GetFeatureIDsRow(formId, ct);

                var notExist = featureIDs.Except(formRowFeatureIDs).ToList();

                if (notExist.Count != 0)
                {
                    var features = await uow.FormFeatures.ListAsync(
                        predicate: f => notExist.Contains(f.Id),
                        asNoTracking: false,
                        ct: ct);

                    uow.FormFeatures.RemoveRange(features);
                    await uow.SaveChangesAsync(ct);
                }

                await uow.CommitTransactionAsync(ct);
                return affected > 0 ? new(FeatureStatus.Created, featureId) : new(FeatureStatus.NoContent, featureId);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<AddEditFeatureToFormResult> EditFeature(
            int featureId,
            int formId,
            string feature,
            int[] rowIds,
            int displayOrder,
            string? color,
            CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(FeatureStatus.FormNotFound, null);

            await uow.BeginTransactionAsync(ct);

            try
            {
                var featureEntity = await uow.FormFeatures.FirstOrDefaultAsync(
                    predicate: f => f.Id == featureId && f.FormId == formId,
                    selector: f => f,
                    asNoTracking: false,
                    ct: ct);

                if (featureEntity == null)
                {
                    await uow.RollbackTransactionAsync(ct);
                    return new(FeatureStatus.FeatureNotFound, null); 
                }

                var nameExists = await uow.FormFeatures.AnyAsync(
                    predicate: f => f.FormId == formId &&
                                    f.Id != featureEntity.Id &&
                                    f.Name.Trim() == feature.Trim(),
                    ct: ct);

                if (nameExists)
                {
                    await uow.RollbackTransactionAsync(ct);
                    return new(FeatureStatus.FormRowNameExist, null);
                }

                var displayOrderExists = await uow.FormFeatures.AnyAsync(
                    predicate: f => f.FormId == formId &&
                                    f.Id != featureEntity.Id &&
                                    f.DisplayOrder == displayOrder,
                    ct: ct);

                if (displayOrderExists)
                {
                    await uow.RollbackTransactionAsync(ct);
                    return new(FeatureStatus.DisplayOrderConflict, null);
                }

                featureEntity.Name = feature;
                featureEntity.DisplayOrder = displayOrder;
                featureEntity.Color = color;

                await uow.SaveChangesAsync(ct);

                var affectedDelete = await uow.Forms.DeleteFormRowsAsync(featureId, ct);
                var affectedAdd = await uow.Forms.AddFormRowsAsync(rowIds, featureId, ct);

                var hasAnyRowsWithFeature = await uow.FormRows.AnyAsync(
                    predicate: r => r.FormFeatureId == featureId,
                    ct: ct);

                if (!hasAnyRowsWithFeature)
                {
                    uow.FormFeatures.Remove(featureEntity);
                    await uow.SaveChangesAsync(ct);
                }

                await uow.CommitTransactionAsync(ct);

                return new(FeatureStatus.Updated, featureEntity.Id);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<AddRowToFormResult> AddRowAndCell(int formId, int featureId, int rowIndex, CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(FeatureStatus.FormNotFound);

            if (featureId != 0 && !await uow.Forms.FeatureExistsAsync(featureId, ct))
                return new(FeatureStatus.FormNotFound);

            if (!await uow.Forms.RowIndexExistsAsync(formId, rowIndex, ct))
                return new(FeatureStatus.FormNotFound);

            await uow.BeginTransactionAsync(ct);
            try
            {
                await uow.Forms.ShiftRowsAsync(formId, rowIndex, ct);
                var rowId = await uow.Forms.CreateRowAsync(formId, featureId, rowIndex, ct);
                await uow.Forms.UpdateFormAsync(formId, StatusForm.Add, ct);
                await uow.Forms.CreateCellsAsync(formId, rowId, ct);
                await uow.CommitTransactionAsync(ct);
                return new(FeatureStatus.NoContent);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw; // ⬅️ let the middleware map exceptions to ProblemDetails/HTTP
            }
        }

        public async Task<RemoveRowResult> RemoveRow(int formId, int rowIndex, CancellationToken ct)
        {
            if (!await uow.Forms.FormExistsAsync(formId, ct))
                return new(FeatureStatus.FormNotFound);

            // Single fetch
            var formRow = await uow.FormRows.FirstOrDefaultAsync(
                predicate: r => r.FormId == formId && r.RowIndex == rowIndex,
                selector: r => r,
                ct: ct);

            if (formRow is null)
                return new(FeatureStatus.FormRowNotFound);

            if (formRow.FormFeatureId != null)
            {
                var rowCount = await uow.FormRows.CountAsync(
                    predicate: r => r.FormId == formId && r.FormFeatureId == formRow.FormFeatureId,
                    ct: ct
                    );

                if (rowCount == 1)
                {
                    var feature = await uow.FormFeatures.FirstOrDefaultAsync(
                        predicate: f => f.FormId == formId && f.Id == formRow.FormFeatureId,
                        selector: f => f,
                        ct: ct
                        );

                    if (feature == null)
                        return new(FeatureStatus.FeatureRowNotFound);

                    uow.FormFeatures.Remove(feature);
                }
            }

            await uow.BeginTransactionAsync(ct);
            try
            {
                uow.FormRows.Remove(formRow);
                await uow.Forms.UpdateFormAsync(formId, StatusForm.Remove, ct);

                await uow.SaveChangesAsync(ct);

                await uow.Forms.ShiftMinusRowsAsync(formId, rowIndex, ct);

                await uow.SaveChangesAsync(ct);

                await uow.CommitTransactionAsync(ct);
                return new(FeatureStatus.NoContent);
            }
            catch
            {
                await uow.RollbackTransactionAsync(ct);
                throw;
            }
        }

    }
}
