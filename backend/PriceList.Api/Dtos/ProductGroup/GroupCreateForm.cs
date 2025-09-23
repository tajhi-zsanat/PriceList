using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductGroup
{
    public record GroupCreateForm(
    [param: Required(ErrorMessage = "نام گروه کالا الزامی است.")]
    [param: StringLength(100, ErrorMessage = "نام گروه کالا نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
    string Name,

    [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
    int DisplayOrder,

    IFormFile? Image,

    [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
    int CategoryId
);
}
