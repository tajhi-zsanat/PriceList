using Microsoft.EntityFrameworkCore;
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
    public class BrandRepository(AppDbContext db) : GenericRepository<Brand>(db), IBrandRepository
    {
        public Task<Brand?> GetByNameAsync(string name, CancellationToken ct = default)
            => Set.AsNoTracking()
                  .FirstOrDefaultAsync(b => b.Name == name, ct);
    }
}
