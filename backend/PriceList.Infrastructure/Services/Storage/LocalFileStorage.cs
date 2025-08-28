using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PriceList.Core.Abstractions.Storage;

namespace PriceList.Infrastructure.Services.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly FileStorageOptions _opt;
        private static readonly HashSet<string> AllowedExts =
            [".jpg", ".jpeg", ".png", ".webp", ".svg", ".pdf"]; 

        public LocalFileStorage(IOptions<FileStorageOptions> options)
        {
            _opt = options.Value ?? throw new ArgumentNullException(nameof(options));
            Directory.CreateDirectory(_opt.PhysicalRoot); 
        }

        public async Task<string> SaveAsync(Stream content, string fileName, string subfolder, CancellationToken ct = default)
        {
            if (content == null || !content.CanRead) throw new InvalidOperationException("Empty stream.");
            if (string.IsNullOrWhiteSpace(fileName)) throw new InvalidOperationException("Filename is required.");

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedExts.Contains(ext)) throw new InvalidOperationException("Unsupported file type.");

            var dir = Path.Combine(_opt.PhysicalRoot, subfolder);
            Directory.CreateDirectory(dir);

            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var safeName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
            var newName = $"{safeName}_{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(dir, newName);

            using (var fs = File.Create(full))
            {
                await content.CopyToAsync(fs, ct);
            }

            return $"{_opt.RequestPath.TrimEnd('/')}/{subfolder}/{newName}".Replace("\\", "/");
        }

        public Task DeleteAsync(string? relativePath, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return Task.CompletedTask;

            var trimmed = relativePath.Replace("\\", "/");
            if (!trimmed.StartsWith(_opt.RequestPath, StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask; // not ours

            var sub = trimmed[_opt.RequestPath.Length..].TrimStart('/'); // e.g., categories/xyz.jpg
            var full = Path.Combine(_opt.PhysicalRoot, sub.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(full)) File.Delete(full);

            return Task.CompletedTask;
        }
    }
}
