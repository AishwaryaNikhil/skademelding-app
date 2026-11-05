namespace TelenorSkademeldingApp.Application.Common.Options;

/// <summary>Configurable limits applied to uploaded files.</summary>
public sealed class FileUploadOptions
{
    public const string SectionName = "FileUpload";

    public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024; // 10 MB

    public int MaxFilesPerCategory { get; set; } = 10;

    public string[] AllowedImageContentTypes { get; set; } =
        ["image/jpeg", "image/png", "image/webp", "image/gif"];

    public string[] AllowedRepairContentTypes { get; set; } =
        ["image/jpeg", "image/png", "image/webp", "image/gif", "application/pdf"];
}
