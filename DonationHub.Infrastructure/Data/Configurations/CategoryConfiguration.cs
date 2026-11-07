using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.NameAr)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.NameEn)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.DescriptionEn)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.DescriptionAr)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.HasMany(c => c.Campaigns)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
