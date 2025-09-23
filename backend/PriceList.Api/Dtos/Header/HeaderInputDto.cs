using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Header
{
    public sealed class HeaderInputDto
    {
        [Required(ErrorMessage = "نام الزامی است.")]
        [StringLength(100, ErrorMessage = "نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        public string Name { get; set; } = default!;

        [Range(1, int.MaxValue, ErrorMessage = "شناسه برند نامعتبر است.")]
        public int BrandId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه نوع کالا نامعتبر است.")]
        public int TypeId { get; set; }
    }
}
