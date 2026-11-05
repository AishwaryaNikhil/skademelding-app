namespace TelenorSkademeldingApp.Domain.Enums;

/// <summary>
/// Lifecycle of a damage claim.
/// </summary>
public enum ClaimStatus
{
    Submitted = 0,
    UnderReview = 1,
    RepairInProgress = 2,
    Repaired = 3,
    Closed = 4,
    Rejected = 5
}
