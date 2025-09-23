using System.ComponentModel.DataAnnotations;

namespace PriceList.Api.Dtos.ProductType
{
    public record TypeUpdateForm(
        [param: Required][param: StringLength(100)] string Name,
        [param: Range(0, 9999)] int DisplayOrder,
        IFormFile? Image,
        List<int> Features,
        [param: Required] int GroupId
    );
}
