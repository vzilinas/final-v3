
namespace Whofax.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "api";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api");
        });

        return app;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }
}
