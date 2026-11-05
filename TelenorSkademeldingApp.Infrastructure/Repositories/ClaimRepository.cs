using Microsoft.EntityFrameworkCore;
using TelenorSkademeldingApp.Application.Common.Interfaces;
using TelenorSkademeldingApp.Domain.Entities;
using TelenorSkademeldingApp.Infrastructure.Persistence;

namespace TelenorSkademeldingApp.Infrastructure.Repositories;

public sealed class ClaimRepository : IClaimRepository
{
    private readonly SkademeldingDbContext _db;

    public ClaimRepository(SkademeldingDbContext db) => _db = db;

    public async Task AddAsync(Claim claim, CancellationToken cancellationToken = default) =>
        await _db.Claims.AddAsync(claim, cancellationToken);

    public Task<Claim?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _db.Claims
            .Include(c => c.DamageType)
            .Include(c => c.DamageCause)
            .Include(c => c.Attachments)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _db.SaveChangesAsync(cancellationToken);
}
