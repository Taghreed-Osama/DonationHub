using DonationHub.Domain.ValueObjects;
using DonationHub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(table=>
            {
                table.HasCheckConstraint(
                    "CK_AspNetUsers_PreferredLanguage",
                    "[PreferredLanguage] IN ('en', 'ar')");
            });
            builder.HasKey(a => a.Id);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(a => a.FullName)
               .IsRequired()
               .HasMaxLength(150);

            builder.Property(a => a.PreferredLanguage)
                .IsRequired()
                .HasMaxLength(2)
                .HasDefaultValue("en");

            builder.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(a => a.Campaigns)
                .WithOne()
                .HasForeignKey(c => c.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Donations)
                .WithOne()
                .HasForeignKey(d => d.DonorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
