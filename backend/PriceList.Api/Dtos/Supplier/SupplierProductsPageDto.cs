using PriceList.Core.Application.Dtos.Product;

namespace PriceList.Api.Dtos.Supplier
{
    public sealed class SupplierProductsPageDto
    {
        public int SupplierId { get; init; }
        public required string SupplierName { get; init; }

        // Products for THIS supplier (paged)
        public required IReadOnlyList<ProductListItemDto> Items { get; init; }

        // Paging state PER supplier
        public required int Page { get; init; }
        public required int PageSize { get; init; }
        public required int TotalCount { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
