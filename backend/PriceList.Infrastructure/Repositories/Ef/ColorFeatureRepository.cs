using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class ColorFeatureRepository : GenericRepository<ColorFeature>, IColorFeatureRepository
    {
        public ColorFeatureRepository(AppDbContext db, ILogger<ColorFeature> logger)
       : base(db, logger)
        {
        }

        public async Task<bool> GetByFeaturesAsync(string IDs, int brandId, int SupplierId, CancellationToken ct = default)
        {
            return await Set.AnyAsync(cf => cf.FeatureIDs == IDs 
            && cf.BrandId == brandId &&
            cf.SupplierId == SupplierId, ct);
        }
    }
}
