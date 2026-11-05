namespace TelenorSkademeldingApp.Domain.Entities;

/// <summary>
/// Lookup describing the cause of the damage (e.g. accident, nature/weather).
/// Stored as a table so new causes can be added without code changes.
/// </summary>
public class DamageCause
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
