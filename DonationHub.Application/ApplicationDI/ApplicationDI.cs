using DonationHub.Application.Features.Authentication.Register;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace DonationHub.Application.ApplicationDI
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddSingleton<TimeProvider>(TimeProvider.System);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(RegisterCommandHandler).Assembly);
            });

            return services;
        }
    }
}
