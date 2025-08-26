using Microsoft.EntityFrameworkCore.Storage;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _tx;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IProductGroupRepository ProductGroups { get; }
        public IProductTypeRepository ProductTypes { get; }
        public IBrandRepository Brands { get; }
        public IErrorLogRepository Errors { get; }

        public UnitOfWork(
            AppDbContext db,
            IProductRepository products,
            ICategoryRepository categories,
            IProductGroupRepository productGroups,
            IProductTypeRepository productTypes,
            IBrandRepository brands,
            IErrorLogRepository errors)
        {
            _db = db;
            Products = products;
            Categories = categories;
            ProductGroups = productGroups;
            ProductTypes = productTypes;
            Brands = brands;
            Errors = errors;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct = default) => _tx = await _db.Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is not null) await _tx.CommitAsync(ct);
            await DisposeTransactionAsync();
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_tx is not null) await _tx.RollbackAsync(ct);
            await DisposeTransactionAsync();
        }

        private async Task DisposeTransactionAsync()
        {
            if (_tx is not null) { await _tx.DisposeAsync(); _tx = null; }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeTransactionAsync();
            await _db.DisposeAsync();
        }
    }
}
