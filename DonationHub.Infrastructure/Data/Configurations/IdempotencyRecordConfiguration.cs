using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
    {
        public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
        {
            builder.ToTable("IdempotencyRecords");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.Key)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(i => i.RequestHash)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(i => i.ResponseStatusCode)
                .IsRequired();

            builder.Property(i => i.ResponseBody)
                .HasColumnType("nvarchar(max)")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(i => i.ExpiresAt)
                .IsRequired();
        }
    }
}
