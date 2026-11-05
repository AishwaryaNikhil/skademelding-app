using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelenorSkademeldingApp.Domain.Entities;

namespace TelenorSkademeldingApp.Infrastructure.Persistence.Configurations;

public sealed class ClaimAttachmentConfiguration : IEntityTypeConfiguration<ClaimAttachment>
{
    public void Configure(EntityTypeBuilder<ClaimAttachment> builder)
    {
        builder.ToTable("ClaimAttachments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever(); // UUID v7 generated in the domain

        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.FileName).HasMaxLength(260).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(128).IsRequired();
        builder.Property(x => x.StoragePath).HasMaxLength(1024).IsRequired();

        builder.HasIndex(x => x.ClaimId);
    }
}
