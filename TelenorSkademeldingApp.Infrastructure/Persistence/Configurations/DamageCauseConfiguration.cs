using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Infrastructure.Persistence.Configurations;

public sealed class DamageCauseConfiguration : IEntityTypeConfiguration<DamageCause>
{
    public void Configure(EntityTypeBuilder<DamageCause> builder)
    {
        builder.ToTable("DamageCauses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(
            new DamageCause { Id = 1, Code = "ACCIDENT", Name = "Ulykke", Description = "Skade forårsaket av en ulykke, f.eks. kollisjon" },
            new DamageCause { Id = 2, Code = "NATURE", Name = "Natur/vær", Description = "Skade forårsaket av vind, storm, lyn eller annet vær" },
            new DamageCause { Id = 3, Code = "FIRE", Name = "Brann", Description = "Skade forårsaket av brann eller røyk" },
            new DamageCause { Id = 4, Code = "THEFT", Name = "Tyveri/hærverk", Description = "Skade forårsaket av tyveri eller hærverk" },
            new DamageCause { Id = 5, Code = "WATER", Name = "Vann/flom", Description = "Skade forårsaket av vannlekkasje eller flom" });
    }
}
