using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Application.Dtos.Product;
using PriceList.Core.Common;
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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db, ILogger<Product> logger)
        : base(db, logger)
        {
        }

        public Task<List<Product>> SearchAsync(string? term, CancellationToken ct = default)
        {
            IQueryable<Product> q = _db.Products.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(term))
                q = q.Where(p => p.Model.Contains(term) ||
                                 (p.Model != null && p.Model.Contains(term)) ||
                                 (p.Description != null && p.Description.Contains(term)));
            return q.OrderByDescending(p => p.Id).ToListAsync(ct);
        }

        public async Task<int> GetTopSupplierAsync(CancellationToken ct = default)
        {
            var supplierId = await _db.Products
                .AsNoTracking()
                .GroupBy(p => p.SupplierId)
                .Select(g => new
                {
                    SupplierId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Select(x => x.SupplierId)
                .FirstOrDefaultAsync(ct);

            return supplierId;
        }
    }
}
