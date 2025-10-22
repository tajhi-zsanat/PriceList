using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Enums
{
    public enum AddColDefStatus
    {
        Initial,
        Created,
        FormNotFound,
        MaxColumnsReached,
        AlreadyExists,
    }

    public sealed record AddColDefResult(AddColDefStatus Status, int? NewColumnIndex = null);
}
