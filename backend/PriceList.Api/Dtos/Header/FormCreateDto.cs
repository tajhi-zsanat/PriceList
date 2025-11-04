using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.Header
{
    public sealed class FormCreateDto
    {
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int GroupId { get; set; }
        //public int TypeId { get; set; }

        public string? FormTitle { get; set; }
        public int Rows { get; set; } = 0;

        /// <summary>
        /// Total number of columns (min 5, max 8).
        /// </summary>
        public int Columns { get; set; } = 5;

        public int DisplayOrder { get; set; } = 0;
    }

}
