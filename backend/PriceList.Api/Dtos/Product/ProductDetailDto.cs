namespace PriceList.Api.Dtos.Product;

public record ProductDetailDto(
    int Id,
    string Name,
    string? Model,
    string? Description,
    long UnitPrice,
    DateTime CreatedAt
);