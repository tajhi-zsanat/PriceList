using PriceList.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos
{

    public record ProductListItemDto(
        int Id,
        string? Model,
        string? Description,
        string? DocumentPath,
        long Price,
        int Number,
        List<string> ProductImages,
        List<ProductCustomPropertyItemDto> CustomProperties
    );

    public record ProductInputDto(
      [param: Required(ErrorMessage = "مدل کالا الزامی است.")]
      [param: StringLength(200, ErrorMessage = "مدل کالا نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.")]
      string Model,

      [param: StringLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیشتر از ۱۰۰۰ کاراکتر باشد.")]
      string? Description,

      string? DocumentPath,

      [param: Range(0, long.MaxValue, ErrorMessage = "قیمت باید بزرگ‌تر یا مساوی صفر باشد.")]
      long Price,

      [param: Range(0, int.MaxValue, ErrorMessage = "تعداد باید بزرگ‌تر یا مساوی صفر باشد.")]
      int Number,

      [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
      int CategoryId,

      [param: Required(ErrorMessage = "انتخاب گروه کالا الزامی است.")]
      int ProductGroupId,

      [param: Required(ErrorMessage = "انتخاب نوع کالا الزامی است.")]
      int ProductTypeId,

      [param: Required(ErrorMessage = "انتخاب برند کالا الزامی است.")]
      int BrandId,

      [param: Required(ErrorMessage = "انتخاب تامین کننده کالا الزامی است.")]
      int SupplierId,

      [param: Required(ErrorMessage = "انتخاب واحد الزامی است.")]
      int UnitId,

      List<ProductCustomPropertyInputDto>? CustomProperties,
      List<string>? ImagePaths
    );

    public record ProductCustomPropertyInputDto(
       [param: Required(ErrorMessage = "کلید ویژگی الزامی است.")]
       [param: StringLength(64, ErrorMessage = "کلید ویژگی نمی‌تواند بیشتر از ۶۴ کاراکتر باشد.")]
       string Key,

       [param: Required(ErrorMessage = "مقدار ویژگی الزامی است.")]
       [param: StringLength(256, ErrorMessage = "مقدار ویژگی نمی‌تواند بیشتر از ۲۵۶ کاراکتر باشد.")]
       string Value
 );

    public record ProductDetailDto(int Id, string Name, string? Model, string? Description, long UnitPrice, DateTime CreatedAt);
    public record ProductCreateDto(string Name, string? Model, string? Description, long UnitPrice);
    public record ProductUpdateDto(string Name, string? Model, string? Description, long UnitPrice);

    public record ProductCustomPropertyItemDto(string Key, string Value);
}
