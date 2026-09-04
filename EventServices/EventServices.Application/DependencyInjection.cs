using EventServices.Application.IServices;
using EventServices.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventServices.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // 1. Register Application Services
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IShowService, ShowService>();

            // 2. Register AutoMapper
            services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

            // 3. Register FluentValidation Validators
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
