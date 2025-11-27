using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? UserName { get; }
        string? IpAddress { get; }
        string? UserAgent { get; }
    }
}
