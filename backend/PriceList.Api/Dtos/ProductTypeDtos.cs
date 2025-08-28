using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos
{
    public record ProductTypeListItemDto(int Id, string? Name, string? ImagePath);

    public record TypeInputDto(
     [param: Required(ErrorMessage = "نام نوع الزامی است.")]
     [param: StringLength(100, ErrorMessage = "نام نوع نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
     string Name,

     [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
     int DisplayOrder,

     string? ImagePath,

    [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
    int GroupId
 );
}
