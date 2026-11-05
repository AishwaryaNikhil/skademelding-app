using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Application.Common.Interfaces;

/// <summary>Read access to the damage type / cause lookup tables.</summary>
public interface ILookupRepository
{
    Task<IReadOnlyList<DamageType>> GetDamageTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DamageCause>> GetDamageCausesAsync(CancellationToken cancellationToken = default);

    Task<bool> DamageTypeExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> DamageCauseExistsAsync(int id, CancellationToken cancellationToken = default);
}
