using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductFeature
{
    public sealed class ProductFeatureAssignDto
    {
        [Required, MinLength(1, ErrorMessage = "حداقل یک شناسه کالا لازم است.")]

        public IReadOnlyList<int> ProductIds { get; init; } = Array.Empty<int>();

        public int BrandId { get; set; }
        public int SupplierId { get; set; }

        [Required, MinLength(1, ErrorMessage = "حداقل یک ویژگی لازم است.")]
        public IReadOnlyList<int> FeatureIds { get; init; } = Array.Empty<int>();

        public string? FeatureColor { get; init; }
    }
}
