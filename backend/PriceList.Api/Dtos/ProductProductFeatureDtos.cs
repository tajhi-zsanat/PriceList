namespace PriceList.Api.Dtos
{
    public sealed class ProductProductFeatureInputDto
    {
        public required IReadOnlyList<int> ProductId { get; set; }
        public required IReadOnlyList<int> productFeatureId { get; set; }
    }
}
