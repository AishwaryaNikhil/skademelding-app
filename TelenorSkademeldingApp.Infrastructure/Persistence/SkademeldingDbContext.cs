using Microsoft.EntityFrameworkCore;
using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Infrastructure.Persistence;

public class SkademeldingDbContext : DbContext
{
    public SkademeldingDbContext(DbContextOptions<SkademeldingDbContext> options) : base(options)
    {
    }

    public DbSet<Claim> Claims => Set<Claim>();

    public DbSet<ClaimAttachment> ClaimAttachments => Set<ClaimAttachment>();

    public DbSet<DamageType> DamageTypes => Set<DamageType>();

    public DbSet<DamageCause> DamageCauses => Set<DamageCause>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkademeldingDbContext).Assembly);
    }
}
