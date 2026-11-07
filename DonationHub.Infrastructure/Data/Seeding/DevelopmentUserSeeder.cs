using DonationHub.Application.Common.Authorization;
using DonationHub.Infrastructure.Identity;
using DonationHub.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;


namespace DonationHub.Infrastructure.Data.Seeding
{
    public static class DevelopmentUserSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();

            var timeProvider = services
                .GetRequiredService<TimeProvider>();

            var options = services
                .GetRequiredService<IOptions<DevelopmentUsersOptions>>()
                .Value;

            await CreateTestUserAsync(
                userManager,
                timeProvider,
                options.CampaignManager,
                Roles.CampaignManager);

            await CreateTestUserAsync(
                userManager,
                timeProvider,
                options.Donor,
                Roles.Donor);
        }

        private static async Task CreateTestUserAsync(
            UserManager<ApplicationUser> userManager,
            TimeProvider timeProvider,
            TestUserOptions options,
            string role)
        {
            if (string.IsNullOrWhiteSpace(options.Email) ||
                string.IsNullOrWhiteSpace(options.FullName) ||
                string.IsNullOrWhiteSpace(options.Password))
            {
                throw new InvalidOperationException(
                    $"Configuration for test user role {role} is missing.");
            }

            var user = await userManager.FindByEmailAsync(options.Email);

            if (user is null)
            {
                user = new ApplicationUser(
                    options.FullName,
                    options.PreferredLanguage,
                    true,
                    timeProvider.GetUtcNow())
                {
                    Email = options.Email,
                    UserName = options.Email
                };

                var createResult = await userManager.CreateAsync(
                    user,
                    options.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to create test user {options.Email}: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var roleResult = await userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign role {role}: {errors}");
                }
            }
        }
    }
}
