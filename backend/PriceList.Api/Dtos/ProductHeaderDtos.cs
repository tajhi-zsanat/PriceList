namespace PriceList.Api.Dtos
{
    public record HeaderListItemDto(int Id, string? Name);
    public sealed class HeaderInputDto
    {
        public required string Name { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
    }
}
