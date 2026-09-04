using EventServices.Application.IRepository;
using EventServices.Infrastructure.Repositiory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Infrastructure
{
    public static class InfrastructureRepositioryExtensions
    {
        public static IServiceCollection AddRepositioryExtension(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository,EventRepository>();
            services.AddScoped<IVenueRepository,VenueRepository>();
            services.AddScoped<IShowRepository,ShowRepository>();

            return services;
        }

    }
}
