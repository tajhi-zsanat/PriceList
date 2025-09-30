namespace PriceList.Core.Abstractions.Services;

public interface IProductPdfService
{
    Task<byte[]> GenerateAsync(int productId, CancellationToken ct);
}