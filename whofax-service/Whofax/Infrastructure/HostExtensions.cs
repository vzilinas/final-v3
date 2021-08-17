using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Whofax.Infrastructure.Persistence;
using Whofax.Application.Common.Helpers;

namespace Whofax.Infrastructure;

public static class HostExtensions
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var servicesScope = host.Services.CreateScope();
        using var context = servicesScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = servicesScope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        while (!context.Database.CanConnect())
        {
            logger.LogWarning("Database not available, retrying again in {delay}", 10);
            AsyncHelper.RunSync(() => Task.Delay(TimeSpan.FromSeconds(10)));
        }
        context.Database.Migrate();
        return host;
    }
}
