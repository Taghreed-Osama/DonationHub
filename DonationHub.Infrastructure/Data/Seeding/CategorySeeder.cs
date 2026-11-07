using DonationHub.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Data.Seeding
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;

            var context = services
                .GetRequiredService<AppDbContext>();

            var timeProvider = services
                .GetRequiredService<TimeProvider>();

            var categories = new[]
            {
                new
                {
                    NameEn = "Health",
                    NameAr = "الصحة",
                    DescriptionEn = "Supporting healthcare services and medical assistance.",
                    DescriptionAr = "دعم الخدمات الصحية وتوفير المساعدات الطبية."
                },
                new
                {
                    NameEn = "Education",
                    NameAr = "التعليم",
                    DescriptionEn = "Supporting education and learning opportunities.",
                    DescriptionAr = "دعم التعليم وتوفير فرص التعلم."
                },
                new
                {
                    NameEn = "Relief",
                    NameAr = "الإغاثة",
                    DescriptionEn = "Providing emergency relief and humanitarian assistance.",
                    DescriptionAr = "تقديم الإغاثة العاجلة والمساعدات الإنسانية."
                },
                new
                {
                    NameEn = "Orphans",
                    NameAr = "الأيتام",
                    DescriptionEn = "Supporting orphan care and their essential needs.",
                    DescriptionAr = "دعم رعاية الأيتام وتوفير احتياجاتهم الأساسية."
                },
                new
                {
                    NameEn = "Environment",
                    NameAr = "البيئة",
                    DescriptionEn = "Supporting environmental protection and sustainability.",
                    DescriptionAr = "دعم حماية البيئة والاستدامة."
                }
            };

            var existingNames = await context.Categories
                .Select(c => c.NameEn)
                .ToListAsync();

            var now = timeProvider.GetUtcNow();

            foreach (var item in categories)
            {
                if (existingNames.Contains(item.NameEn))
                    continue;

                var category = new Category(
                    item.NameAr,
                    item.NameEn,
                    item.DescriptionAr,
                    item.DescriptionEn,
                    now);

                await context.Categories.AddAsync(category);
            }

            await context.SaveChangesAsync();
        }
    }
}
