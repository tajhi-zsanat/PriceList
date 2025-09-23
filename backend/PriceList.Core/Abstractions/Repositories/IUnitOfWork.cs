using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IProductGroupRepository ProductGroups { get; }
        IProductTypeRepository ProductTypes { get; }
        IBrandRepository Brands { get; }
        IErrorLogRepository Errors { get; }
        IUnitRepository Units { get; }
        ISupplierRepository Suppliers { get; }
        IFeatureRepository Features { get; }
        IColorFeatureRepository ColorFeatures { get; }
        IProductTypeFeaturesRepository ProductTypeFeatures { get; }
        IProductFeatureRepository ProductFeatures { get; }
        IProductImageRepository ProductImageRepository { get; }
        IHeaderRepository Header { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        // Optional transaction helpers
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
