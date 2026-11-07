using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.TitleEn)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.TitleAr)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.DescriptionEn)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.DescriptionAr)
                .HasMaxLength(500)
                .IsRequired();

            builder.OwnsOne(c => c.GoalAmount, money =>
            {
                money.Property(m => m.Amount)
                .HasColumnName("GoalAmount")
                .HasPrecision(18, 2)
                .IsRequired();

                money.Property(m => m.Currency)
                .HasColumnName("GoalCurrency")
                .HasMaxLength(3)
                .IsRequired();
            });

            builder.OwnsOne(c => c.RaisedAmount, money =>
            {
                money.Property(m => m.Amount)
                .HasColumnName("RaisedAmount")
                .HasPrecision(18, 2)
                .IsRequired();

                money.Property(m => m.Currency)
                .HasColumnName("RaisedCurrency")
                .HasMaxLength(3)
                .IsRequired();
            });

            builder.Navigation(c => c.GoalAmount)
                .IsRequired();

            builder.Navigation(c => c.RaisedAmount)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.BannerFileName)
                .HasMaxLength(200);

            builder.Property(c => c.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.HasQueryFilter(c => c.IsDeleted == false);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Campaigns)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                .WithMany(a => a.Campaigns)
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Donations)
                .WithOne(d => d.Campaign)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.RowVersion)
                .IsRowVersion();
        }
    }
}
