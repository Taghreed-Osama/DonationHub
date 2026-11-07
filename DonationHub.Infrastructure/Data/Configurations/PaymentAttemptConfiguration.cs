using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.ToTable("PaymentAttempts");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.DonationId)
                .IsRequired();

            builder.Property(p => p.StripeCheckoutSessionId)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(p => p.StripeCheckoutSessionId)
                .IsUnique()
                .HasFilter("[StripeCheckoutSessionId] IS NOT NULL");

            builder.Property(p => p.StripePaymentIntentId)
                .HasMaxLength(255);

            builder.HasIndex(p => p.StripePaymentIntentId)
                .IsUnique()
                .HasFilter("[StripePaymentIntentId] IS NOT NULL");

            builder.OwnsOne(p => p.Amount, money =>
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

            builder.Navigation(p => p.Amount)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired();

            builder.Property(p => p.FailureCode)
                .HasMaxLength(200);
        }
    }
}
