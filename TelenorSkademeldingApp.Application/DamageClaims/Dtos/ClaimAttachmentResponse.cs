namespace TelenorSkademeldingApp.Application.DamageClaims.Dtos;

public sealed record ClaimAttachmentResponse(
    Guid Id,
    string Type,
    string FileName,
    string ContentType,
    long SizeBytes,
    DateTime UploadedAtUtc);
