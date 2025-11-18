using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IFormRowRepository : IGenericRepository<FormRow>
    {
        Task<int> GetFormByMaxRow(List<int> ids, CancellationToken ct);
        Task<int> GetCountRow(List<int> ids, CancellationToken ct);
    }
}
