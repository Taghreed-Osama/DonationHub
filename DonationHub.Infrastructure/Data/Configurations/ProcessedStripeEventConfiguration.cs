using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class ProcessedStripeEventConfiguration : IEntityTypeConfiguration<ProcessedStripeEvent>
    {
        public void Configure(EntityTypeBuilder<ProcessedStripeEvent> builder)
        {
            builder.ToTable("ProcessedStripeEvents");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.EventId)
                .IsRequired();

            builder.Property(p => p.Type)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.ProcessedAt)
                .IsRequired();
        }
    }
}
