using Microsoft.EntityFrameworkCore;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Domain.Entities;
using TelenorSkademeldingApp.Infrastructure.Persistence;

namespace TelenorSkademeldingApp.Infrastructure.Repositories;

public sealed class LookupRepository : ILookupRepository
{
    private readonly SkademeldingDbContext _db;

    public LookupRepository(SkademeldingDbContext db) => _db = db;

    public async Task<IReadOnlyList<DamageType>> GetDamageTypesAsync(CancellationToken cancellationToken = default) =>
        await _db.DamageTypes.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<DamageCause>> GetDamageCausesAsync(CancellationToken cancellationToken = default) =>
        await _db.DamageCauses.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<bool> DamageTypeExistsAsync(int id, CancellationToken cancellationToken = default) =>
        _db.DamageTypes.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);

    public Task<bool> DamageCauseExistsAsync(int id, CancellationToken cancellationToken = default) =>
        _db.DamageCauses.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);
}
