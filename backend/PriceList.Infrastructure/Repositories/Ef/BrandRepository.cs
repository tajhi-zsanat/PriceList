using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext db, ILogger<Brand> logger)
        : base(db, logger)
        {
        }

        public Task<Brand?> GetByNameAsync(string name, CancellationToken ct = default)
            => _db.Brands.AsNoTracking()
                  .FirstOrDefaultAsync(b => b.Name == name, ct);
    }
}
