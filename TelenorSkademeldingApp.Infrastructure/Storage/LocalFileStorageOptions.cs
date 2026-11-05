namespace TelenorSkademeldingApp.Infrastructure.Storage;

/// <summary>Options for the local-disk file storage implementation.</summary>
public sealed class LocalFileStorageOptions
{
    public const string SectionName = "FileStorage";

    /// <summary>Root directory where uploaded files are written.</summary>
    public string RootPath { get; set; } = "App_Data/uploads";
}
