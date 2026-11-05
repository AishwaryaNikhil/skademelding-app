namespace TelenorSkademeldingApp.Domain.Entities;

/// <summary>
/// Lookup describing what kind of thing was damaged (e.g. car, building, wind-affected area).
/// Stored as a table so new types can be added without code changes.
/// </summary>
public class DamageType
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
