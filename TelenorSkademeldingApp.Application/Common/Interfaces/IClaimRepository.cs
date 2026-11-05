using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Application.Common.Interfaces;

/// <summary>Persistence operations for <see cref="Claim"/> aggregates.</summary>
public interface IClaimRepository
{
    Task AddAsync(Claim claim, CancellationToken cancellationToken = default);

    Task<Claim?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
