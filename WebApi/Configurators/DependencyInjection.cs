using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Entities;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Configurators
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<InstallationService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<UserService>();

            return services;
        }
    }
}
