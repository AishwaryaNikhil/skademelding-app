namespace TelenorSkademeldingApp.Application.DamageClaims.Dtos;

public sealed record ClaimResponse(
    Guid Id,
    string Reference,
    int DamageTypeId,
    string DamageTypeName,
    int DamageCauseId,
    string DamageCauseName,
    string Summary,
    string Status,
    string? ReportedBy,
    DateTime CreatedAtUtc,
    IReadOnlyList<ClaimAttachmentResponse> Attachments);
