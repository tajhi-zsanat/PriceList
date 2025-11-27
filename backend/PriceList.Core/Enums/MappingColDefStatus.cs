using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Enums
{
    public enum MappingColDefStatus
    {
        Initial,
        Created,
        FormNotFound,
        MaxColumnsReached,
        AlreadyExists,
    }

    public sealed record AddColDefResult(MappingColDefStatus Status, int? NewColumnIndex = null);

    public enum RemoveColDefStatus
    {
        Initial,
        FormNotFound,
        MaxColumnsReached,
        AlreadyExists,
        ColumnNotFound,
        InvalidColumn,
        NoContent,
    }

    public sealed record RemoveColDefResult(RemoveColDefStatus Status);

    public enum FeatureStatus
    {
        Initial,
        FormNotFound,
        MaxColumnsReached,
        AlreadyExists,
        ColumnNotFound,
        InvalidColumn,
        NoContent,
        DisplayOrderConflict,
        AlreadyAssigned,
        Created,
        IsExistFeature,
        InvalidRow,
        FormRowNotFound,
        FeatureRowNotFound,
        FormRowNameExist,
        Updated,
        FeatureNotFound,
    }

    public sealed record AddEditFeatureToFormResult(FeatureStatus Status, int? FeatureId);
    public sealed record AddRowToFormResult(FeatureStatus Status);
    public sealed record RemoveRowResult(FeatureStatus Status);
}
