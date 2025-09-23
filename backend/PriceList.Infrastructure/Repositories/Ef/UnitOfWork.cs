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
        public IUnitRepository Units { get; }
        public ISupplierRepository Suppliers { get; }
        public IFeatureRepository Features { get; }
        public IColorFeatureRepository ColorFeatures { get; }
        public IProductTypeFeaturesRepository ProductTypeFeatures { get; }
        public IProductFeatureRepository ProductFeatures { get; }
        public IProductImageRepository ProductImageRepository { get; }
        public IHeaderRepository Header { get; }

        public UnitOfWork(
            AppDbContext db,
            IProductRepository products,
            ICategoryRepository categories,
            IProductGroupRepository productGroups,
            IProductTypeRepository productTypes,
            IBrandRepository brands,
            IErrorLogRepository errors,
            IUnitRepository units,
            ISupplierRepository suppliers,
            IFeatureRepository features,
            IProductTypeFeaturesRepository productTypeFeatures,
            IProductFeatureRepository productFeatures,
            IProductImageRepository productImageRepository,
            IHeaderRepository productHeader,
            IColorFeatureRepository colorFeatures)
        {
            _db = db;
            Products = products;
            Categories = categories;
            ProductGroups = productGroups;
            ProductTypes = productTypes;
            Brands = brands;
            Errors = errors;
            Units = units;
            Suppliers = suppliers;
            Features = features;
            ProductTypeFeatures = productTypeFeatures;
            ProductFeatures = productFeatures;
            ProductImageRepository = productImageRepository;
            Header = productHeader;
            ColorFeatures = colorFeatures;
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
