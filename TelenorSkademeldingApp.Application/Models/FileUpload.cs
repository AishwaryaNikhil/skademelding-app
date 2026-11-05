namespace TelenorSkademeldingApp.Application.Models;

/// <summary>
/// Transport-agnostic representation of an uploaded file, decoupled from ASP.NET's IFormFile
/// so the Application layer has no dependency on the web host.
/// </summary>
public sealed class FileUpload
{
    public required Stream Content { get; init; }

    public required string FileName { get; init; }

    public required string ContentType { get; init; }

    public long Length { get; init; }
}
