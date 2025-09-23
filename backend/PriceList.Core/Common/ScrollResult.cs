using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Common
{
    /// <summary>
    /// Result model for infinite-scroll (offset/limit) scenarios.
    /// </summary>
    public sealed class ScrollResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }

        /// <summary>How many records were skipped by the query (offset).</summary>
        public required int Skip { get; init; }

        /// <summary>Requested batch size (limit).</summary>
        public required int Take { get; init; }

        /// <summary>How many items are actually returned in this batch.</summary>
        public required int ReturnedCount { get; init; }

        /// <summary>Total items in the full result (optional to compute/send).</summary>
        public int? TotalCount { get; init; }

        /// <summary>Whether there are more items after this slice.</summary>
        public bool HasMore => !TotalCount.HasValue || (Skip + ReturnedCount) < TotalCount.Value;
    }
}
