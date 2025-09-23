using PriceList.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IColorFeatureRepository : IGenericRepository<ColorFeature>
    {
        Task<bool> GetByFeaturesAsync(string IDs, int brandId, int SupplierId, CancellationToken ct = default);
    }
}
