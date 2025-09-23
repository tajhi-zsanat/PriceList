using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductGroup
{
    public record GroupUpdateForm(
    [param: Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
    [param: StringLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
    string Name,

    [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
    int DisplayOrder,

    IFormFile? Image,

    [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
    int CategoryId
);
}
