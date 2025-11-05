namespace PriceList.Api.Dtos.ProductGroup
{
    public sealed class AddGroupToRowsDto
    {
        public int FormId { get; set; }
        public int GroupId { get; set; }
        public IReadOnlyList<int> RowIds { get; set; } = default!;
        public int DisplayOrder { get; set; }
        public string? Color { get; set; }
    }
}
