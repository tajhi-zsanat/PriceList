using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Brand
{
    public record BrandInputDto(
        [param: Required(ErrorMessage = "نام برند الزامی است.")]
        [param: StringLength(100, ErrorMessage = "نام برند نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        string Name,

        [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        int DisplayOrder,

        string? ImagePath
);
}
