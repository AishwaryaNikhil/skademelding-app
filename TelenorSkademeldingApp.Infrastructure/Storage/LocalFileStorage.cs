using Microsoft.Extensions.Options;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Application.Models;

namespace TelenorSkademeldingApp.Infrastructure.Storage;

/// <summary>
/// Stores uploaded files on the local file system. Swap this implementation for a blob/S3
/// backed one in production without changing the Application layer.
/// </summary>
public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public LocalFileStorage(IOptions<LocalFileStorageOptions> options)
    {
        _rootPath = Path.GetFullPath(options.Value.RootPath);
    }

    public async Task<StoredFile> SaveAsync(FileUpload file, string subPath, CancellationToken cancellationToken = default)
    {
        var safeSubPath = SanitizeSubPath(subPath);
        var directory = Path.Combine(_rootPath, safeSubPath);
        Directory.CreateDirectory(directory);

        // Only keep short alphanumeric extensions; the user-supplied file name is never used on disk.
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension.Length > 10 || !extension.Skip(1).All(char.IsAsciiLetterOrDigit))
        {
            extension = string.Empty;
        }
        var storedName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(directory, storedName);

        await using (var target = File.Create(absolutePath))
        {
            await file.Content.CopyToAsync(target, cancellationToken);
        }

        var size = new FileInfo(absolutePath).Length;
        var relativePath = Path.Combine(safeSubPath, storedName).Replace('\\', '/');
        return new StoredFile(relativePath, size);
    }

    private static string SanitizeSubPath(string subPath)
    {
        // Prevent path traversal by stripping any directory-navigation segments.
        var segments = subPath
            .Split(['/', '\\'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => s != "." && s != "..")
            .Select(s => string.Concat(s.Where(c => !Path.GetInvalidFileNameChars().Contains(c))));

        return Path.Combine(segments.ToArray());
    }
}
