namespace TelenorSkademeldingApp.Domain.Abstractions;

/// <summary>
/// Base type for entities that use a GUID primary key and carry audit timestamps.
/// Ids are UUID v7 (time-ordered) so inserts stay sequential in clustered/B-tree indexes.
/// </summary>
public abstract class AuditableEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }
}
