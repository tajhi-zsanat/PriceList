namespace PriceList.Api.Dtos
{

    public record ProductListItemDto(int Id, string? Model, decimal UnitPrice);
    public record ProductDetailDto(int Id, string Name, string? Model, string? Description, long UnitPrice, DateTime CreatedAt);
    public record ProductCreateDto(string Name, string? Model, string? Description, long UnitPrice);
    public record ProductUpdateDto(string Name, string? Model, string? Description, long UnitPrice);
}
