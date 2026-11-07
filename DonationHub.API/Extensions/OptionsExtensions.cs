using DonationHub.Application.Options;
using DonationHub.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace DonationHub.API.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddApplicationOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<SeedAdminOptions>()
                .Bind(configuration.GetSection(SeedAdminOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DonationOptions>()
                .Bind(configuration.GetSection(DonationOptions.SectionName))
                .ValidateDataAnnotations()
                .Validate(
                    options => options.MinAmount <= options.MaxAmount,
                    "MinAmount cannot be greater than MaxAmount")
                .ValidateOnStart();

            return services;
        }
    }
}
