using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Infrastructure.Persistence.Configurations;

public sealed class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.ToTable("Claims");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever(); // UUID v7 generated in the domain

        builder.Property(x => x.Reference).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.Reference).IsUnique();

        builder.Property(x => x.Summary).HasMaxLength(4000).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.ReportedBy).HasMaxLength(256);

        builder.HasOne(x => x.DamageType)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.DamageTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DamageCause)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.DamageCauseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.Claim)
            .HasForeignKey(x => x.ClaimId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ReportedBy);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CreatedAtUtc);
    }
}
