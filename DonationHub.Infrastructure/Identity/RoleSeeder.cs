using DonationHub.Application;
using DonationHub.Application.Common.Authorization;
using DonationHub.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.Identity
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;

            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();

            //var configuration = services
            //    .GetRequiredService<IConfiguration>();

            var adminOptions = services
                .GetRequiredService<IOptions<SeedAdminOptions>>().Value;

            var timeprovider = services
                .GetRequiredService<TimeProvider>();

            var roles = new[]
            {
                Roles.Admin,
                Roles.CampaignManager,
                Roles.Donor
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole<Guid>(role));

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Failed to create role {role}: {errors}");
                    }
                }
            }

            //var adminEmail = configuration["SeedAdmin:Email"];
            //var adminFullName = configuration["SeedAdmin:FullName"];
            //var adminPassword = configuration["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(adminOptions.Email) ||
               string.IsNullOrWhiteSpace(adminOptions.FullName) ||
               string.IsNullOrWhiteSpace(adminOptions.Password))
            {
                throw new InvalidOperationException(
                    "Admin seed configuration is missing.");
            }

            var admin = await userManager.FindByEmailAsync(adminOptions.Email);

            if(admin is null)
            {
                admin = new ApplicationUser(
                    adminOptions.Email,
                    "en",
                    true,
                    timeprovider.GetUtcNow())
                {
                    Email = adminOptions.Email,
                    UserName = adminOptions.Email
                };

                var createResult = await userManager.CreateAsync(admin, adminOptions.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Failed to create Admin user: {errors}");
                }
            }

            if(!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                var roleResult = await userManager.AddToRoleAsync(admin,Roles.Admin);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Failed to assign Admin role: {errors}");
                }
            }
        }
    }
}

