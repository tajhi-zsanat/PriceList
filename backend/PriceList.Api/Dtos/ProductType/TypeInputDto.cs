using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductType
{
    public record TypeInputDto
    {
        [Required(ErrorMessage = "نام نوع الزامی است.")]
        [StringLength(100, ErrorMessage = "نام نوع نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        public string Name { get; init; } = null!;

        [Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        public int DisplayOrder { get; init; }

        public string? ImagePath { get; init; }

        [Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
        public int GroupId { get; init; }

        public List<int> Features { get; init; } = new();
    }
}
