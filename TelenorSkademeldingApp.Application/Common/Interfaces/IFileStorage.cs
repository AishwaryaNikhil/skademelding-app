using TelenorSkademeldingApp.Application.Models;

namespace TelenorSkademeldingApp.Application.Common.Interfaces;

/// <summary>
/// Abstraction over binary file storage. The local implementation writes to disk; a production
/// implementation can target Azure Blob Storage / S3 without touching the Application layer.
/// </summary>
public interface IFileStorage
{
    Task<StoredFile> SaveAsync(FileUpload file, string subPath, CancellationToken cancellationToken = default);
}
