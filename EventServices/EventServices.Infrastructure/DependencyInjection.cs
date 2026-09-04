using EventServices.Application.IRepository;
using EventServices.Infrastructure.Data;
using EventServices.Infrastructure.Repositiory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventServices.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register DbContext with SQL Server
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // 2. Register Repositories
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<IShowRepository, ShowRepository>();

            return services;
        }
    }
}
