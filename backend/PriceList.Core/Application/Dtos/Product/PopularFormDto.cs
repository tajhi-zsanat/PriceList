using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Application.Dtos.Product;

public class PopularFormDto
{
    public int Id { get; set; }
    public string FormTitle { get; set; } = default!;
    public string? ImagePath { get; set; }
    public required string UpdateDate { get; set; }
    public int RowCountWithDescriptionAndPrice { get; set; }
    public int ViewCount { get; set; }
    public int CategoryId { get; set; }
    public int ProductGroupId { get; set; }
    public int BrandId { get; set; }
    public DateTime UpdateDateAndTime { get; set; }
}

