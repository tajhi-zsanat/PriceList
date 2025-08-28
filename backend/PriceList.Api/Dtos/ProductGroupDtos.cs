using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos
{
    public record ProductGroupListItemDto(int Id, string? Name, string? ImagePath);

    public record GroupInputDto(
        [param: Required(ErrorMessage = "نام گروه الزامی است.")]
        [param: StringLength(100, ErrorMessage = "نام گروه نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        string Name,

        [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        int DisplayOrder,

         string? ImagePath,

        [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
        int CategoryId
     );

    public sealed record ProductGroupDetailDto(
    int Id,
    string Name,
    string? ImagePath,
    int CategoryId,
    int DisplayOrder);
}
