using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Category
{
    public record CategoryInputDto(
        [param: Required(ErrorMessage = "نام الزامی است.")]
        [param: StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        string Name,

        [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        int DisplayOrder,

        string? ImagePath
);
}
