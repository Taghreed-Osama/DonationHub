using DonationHub.Application.Interfaces.IIdentityService;
using DonationHub.Application.Interfaces.UnitOfWork;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.Interceptors;
using DonationHub.Infrastructure.Identity;
using DonationHub.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.InfrastructureDI
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<SoftDeleteInterceptor>();

            services.AddDbContext<AppDbContext>((ServiceProvider,options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.AddInterceptors(
                    ServiceProvider.GetRequiredService<SoftDeleteInterceptor>());
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.Configure<SeedAdminOptions>(
                configuration.GetSection(SeedAdminOptions.SectionName));

            services.Configure<DevelopmentUsersOptions>(
                configuration.GetSection(DevelopmentUsersOptions.SectionName));

            return services;
        }
    }
}
