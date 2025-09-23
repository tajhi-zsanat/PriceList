using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductHeader;

public sealed class ProductHeaderInputDto
{
    [Required] public string Key { get; set; } = default!;
    public string? Value { get; set; }
}
