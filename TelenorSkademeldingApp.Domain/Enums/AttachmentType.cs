namespace TelenorSkademeldingApp.Domain.Enums;

/// <summary>
/// Distinguishes files captured at damage time from files produced after repair.
/// </summary>
public enum AttachmentType
{
    /// <summary>Image of the damage site (skadested).</summary>
    DamageSiteImage = 0,

    /// <summary>Document or image produced after repair (etter reparasjon).</summary>
    RepairDocument = 1
}
