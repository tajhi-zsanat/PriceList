using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Feature
{
    public sealed class FeatureCreateDto
    {
        [MinLength(1, ErrorMessage = "حداقل یک ردیف لازم است.")]
        public required List<int> RowIds { get; set; }

        [MinLength(1, ErrorMessage = "حداقل یک ویژگی لازم است.")]
        public required List<int> FeatureIds { get; set; }

        public int DisplayOrder { get; set; }

        // Optional hex color like #AABBCC or #ABC
        [RegularExpression("^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "کد رنگ نامعتبر است.")]
        public string? Color { get; set; }
    }
}
