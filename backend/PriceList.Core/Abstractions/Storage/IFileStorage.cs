namespace PriceList.Core.Abstractions.Storage
{
    public interface IFileStorage
    {
        // returns a public path like /uploads/categories/file.jpg (store in DB)
        Task<string> SaveAsync(Stream content, string fileName, string subfolder, CancellationToken ct = default);
        Task DeleteAsync(string? relativePath, CancellationToken ct = default);
    }
}
