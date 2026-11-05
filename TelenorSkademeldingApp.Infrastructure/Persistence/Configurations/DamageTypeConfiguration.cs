using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Infrastructure.Persistence.Configurations;

public sealed class DamageTypeConfiguration : IEntityTypeConfiguration<DamageType>
{
    public void Configure(EntityTypeBuilder<DamageType> builder)
    {
        builder.ToTable("DamageTypes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new DamageType { Id = 1, Code = "CAR", Name = "Skade på bil", Description = "Skade på bil eller annet kjøretøy" },
            new DamageType { Id = 2, Code = "PROPERTY", Name = "Skade på bygning/eiendom", Description = "Skade på bygning, hus eller annen eiendom" },
            new DamageType { Id = 3, Code = "WIND_AREA", Name = "Område rammet av vind", Description = "Uteområde rammet av vind eller storm" },
            new DamageType { Id = 4, Code = "OTHER", Name = "Annet", Description = "Annen type skade" });
    }
}
