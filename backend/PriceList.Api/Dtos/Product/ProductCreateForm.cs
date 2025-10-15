using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Product
{
    public sealed class ProductCreateForm
    {
        [StringLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیشتر از ۱۰۰۰ کاراکتر باشد.")]
        public required string Description { get; set; }

        public string? DocumentPath { get; set; }  // usually server-generated; optional to receive

        [Range(0, long.MaxValue, ErrorMessage = "قیمت باید بزرگ‌تر یا مساوی صفر باشد.")]
        public long Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه دسته‌بندی نامعتبر است.")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه گروه کالا نامعتبر است.")]
        public int ProductGroupId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه نوع کالا نامعتبر است.")]
        public int ProductTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه برند نامعتبر است.")]
        public int BrandId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه تامین‌کننده نامعتبر است.")]
        public int SupplierId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "شناسه واحد نامعتبر است.")]
        public int UnitId { get; set; }

        public List<IFormFile>? Image { get; set; }
    }
}
