using Microsoft.EntityFrameworkCore;
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
    public class ProductRepository(AppDbContext db) : GenericRepository<Product>(db), IProductRepository
    {
        public Task<List<Product>> SearchAsync(string? term, CancellationToken ct = default)
        {
            IQueryable<Product> q = Db.Products.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(term))
                q = q.Where(p => p.Model.Contains(term) ||
                                 (p.Model != null && p.Model.Contains(term)) ||
                                 (p.Description != null && p.Description.Contains(term)));
            return q.OrderByDescending(p => p.Id).ToListAsync(ct);
        }
    }
}
