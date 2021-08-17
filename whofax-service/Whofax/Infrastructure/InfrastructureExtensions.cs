using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whofax.Infrastructure.Persistence;

namespace Whofax.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));
        });

        return services;
    }
}
