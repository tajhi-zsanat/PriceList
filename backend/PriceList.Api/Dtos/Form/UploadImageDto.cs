namespace PriceList.Api.Dtos.Form
{
    public sealed class UploadImageDto
    {
        public int FormId { get; set; }
        public int Id { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
