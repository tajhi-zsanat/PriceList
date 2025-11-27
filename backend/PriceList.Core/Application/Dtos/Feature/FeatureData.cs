namespace PriceList.Core.Application.Dtos.Feature;

public record FeatureData(int Id,
    string Name,
    int DisplayOrder,
    string Color,
    List<int> SelectedValue);
