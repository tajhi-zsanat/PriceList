using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos
{
    public record ProductTypeListItemDto(int Id, string? Name, string? ImagePath, List<ProductFeatures> Features);

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
    };

    public record TypeCreateForm(
        [param: Required(ErrorMessage = "نام نوع الزامی است.")]
        [param: StringLength(100, ErrorMessage = "نام نوع نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        string Name,

        [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
        int DisplayOrder,

        IFormFile? Image,

        List<int> Features,

        [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
        int GroupId
    );

    public record TypeUpdateForm(
    [param: Required(ErrorMessage = "نام نوع الزامی است.")]
    [param: StringLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
    string Name,

    [param: Range(0, 9999, ErrorMessage = "ترتیب نمایش باید بین ۰ تا ۹۹۹۹ باشد.")]
    int DisplayOrder,

    IFormFile? Image,

    List<int> Features,

    [param: Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است.")]
    int GroupId
    );

    public sealed record ProductTypeDetailDto(
        int Id,
        string Name,
        string? ImagePath,
        int GroupId,
        List<int> Features,
        int DisplayOrder);

    public sealed record ProductFeatures(
    int Id,
    string Name);
};
