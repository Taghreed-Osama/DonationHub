using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donations");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.CreatedAt)
                .IsRequired();

            builder.OwnsOne(d => d.Amount, money =>
            {
                money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

                money.Property(m => m.Currency)
                .HasColumnName("AmountCurrency")
                .HasMaxLength(3)
                .IsRequired();
            });

            builder.Navigation(d => d.Amount)
                .IsRequired();

            builder.Property(d => d.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(d => d.CompletedAt)
                .IsRequired();

            builder.Property(d => d.FailureReason)
                .HasMaxLength(300);

            builder.Property(d => d.ReceiptNumber)
                .HasMaxLength(50);

            builder.Property(d => d.ReceiptFileName)
                .HasMaxLength(150);

            builder.HasOne<ApplicationUser>()
                .WithMany(a => a.Donations)
                .HasForeignKey(d => d.DonorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Campaign)
                .WithMany(c => c.Donations)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
