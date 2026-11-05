using TelenorSkademeldingApp.Domain.Abstractions;
using TelenorSkademeldingApp.Domain.Enums;

namespace TelenorSkademeldingApp.Domain.Entities;

/// <summary>
/// A file linked to a claim. Binary content lives in file/blob storage; only metadata is kept here.
/// Keeping attachments in their own table lets a claim carry any number of images and repair files.
/// </summary>
public class ClaimAttachment : AuditableEntity
{
    public Guid ClaimId { get; set; }

    public Claim? Claim { get; set; }

    public AttachmentType Type { get; set; }

    public required string FileName { get; set; }

    public required string ContentType { get; set; }

    /// <summary>Relative path or blob key where the binary is stored.</summary>
    public required string StoragePath { get; set; }

    public long SizeBytes { get; set; }
}
