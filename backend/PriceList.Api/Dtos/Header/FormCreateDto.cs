using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Header
{
    public class FormCreateDto
    {
        public string? FormTitle { get; set; }
        public int CategoryId { get; set; }
        public int GroupId { get; set; }
        public int TypeId { get; set; }
        public int BrandId { get; set; }
        public int DisplayOrder { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
    }
}
