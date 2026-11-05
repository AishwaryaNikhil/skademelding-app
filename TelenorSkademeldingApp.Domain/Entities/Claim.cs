using TelenorSkademeldingApp.Domain.Abstractions;
using TelenorSkademeldingApp.Domain.Enums;

namespace TelenorSkademeldingApp.Domain.Entities;

/// <summary>
/// A reported damage claim (skademelding).
/// </summary>
public class Claim : AuditableEntity
{
    /// <summary>Short human-readable reference derived from <see cref="AuditableEntity.Id"/>.</summary>
    public required string Reference { get; set; }

    public int DamageTypeId { get; set; }

    public DamageType? DamageType { get; set; }

    public int DamageCauseId { get; set; }

    public DamageCause? DamageCause { get; set; }

    public required string Summary { get; set; }

    public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

    /// <summary>Identifier of the authenticated user who reported the claim.</summary>
    public string? ReportedBy { get; set; }

    public ICollection<ClaimAttachment> Attachments { get; set; } = new List<ClaimAttachment>();
}
