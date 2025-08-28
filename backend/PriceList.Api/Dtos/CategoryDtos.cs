using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos
{
    public record CategoryListItemDto(int Id, string? Name, string? ImagePath);

    public record CategoryInputDto(
    [param: Required(ErrorMessage = "نام الزامی است.")]
    [param: StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
    string Name,

    [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
    int DisplayOrder,

    string? ImagePath
    );

    public record CategoryCreateForm(
        [param: Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
        [param: StringLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        string Name,

        [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        int DisplayOrder,

        IFormFile? Image
    );

    public record CategoryUpdateForm(
    string Name,
    int DisplayOrder,
    IFormFile? Image 
    );
}
