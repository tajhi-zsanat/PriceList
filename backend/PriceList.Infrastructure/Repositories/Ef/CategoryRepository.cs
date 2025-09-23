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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db, ILogger<Category> logger)
        : base(db, logger)
        {
        }

        public Task<Category?> GetByNameAsync(string name, CancellationToken ct = default)
            => _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, ct);
    }
}
