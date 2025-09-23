using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Product;

public record ProductUpdateDto(
    [property: Required, StringLength(100)] string Name,
    string? Model,
    [property: StringLength(1000)] string? Description,
    [property: Range(0, long.MaxValue)] long UnitPrice
);