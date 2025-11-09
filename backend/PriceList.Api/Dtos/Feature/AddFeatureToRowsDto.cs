namespace PriceList.Api.Dtos.Feature
{
    public sealed class AddFeatureToRowsDto
    {
        public int FormId { get; set; }
        public string? Feature { get; set; }
        public IReadOnlyList<int> RowIds { get; set; } = default!;
        public int DisplayOrder { get; set; }
        public string? Color { get; set; }
    }
}
