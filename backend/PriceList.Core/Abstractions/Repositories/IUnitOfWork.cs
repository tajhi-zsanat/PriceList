using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Abstractions.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICategoryRepository Categories { get; }
        IProductGroupRepository ProductGroups { get; }
        IProductTypeRepository ProductTypes { get; }
        IBrandRepository Brands { get; }
        IErrorLogRepository Errors { get; }
        IUnitRepository Units { get; }
        //ISupplierRepository Suppliers { get; }
        IFormRepository Forms { get; }
        IFormCellRepository FormCells { get; }
        IFormColumnDefRepository FormColumns { get; }
        IFormRowRepository FormRows { get; }
        IFormFeatureRepository FormFeatures { get; }
        IAuditLogger auditLogger { get; }
        public IFormViewRepository FormViews { get; }
        ICurrentUserService currentUserService { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        // Optional transaction helpers
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
