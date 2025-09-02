namespace PriceList.Api.Dtos
{
    public sealed class SupplierSummaryDto
    {
        public int SupplierId { get; init; }
        public required string SupplierName { get; init; }
        public int ProductCount { get; init; }
    }
}
