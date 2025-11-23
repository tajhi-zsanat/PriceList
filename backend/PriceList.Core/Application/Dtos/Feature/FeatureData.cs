namespace PriceList.Core.Application.Dtos.Feature;

public record FeatureData(int Id,
    string Name,
    int DisplayOrder,
    List<int> SelectedValue);
