using EventServices.Application.IServices;
using EventServices.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddServiceExrension(this IServiceCollection services) {

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IShowService, ShowService>();

            return services;
        }
    }
}
