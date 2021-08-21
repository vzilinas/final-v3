using Whofax.Api.Common.Filters;
using Whofax.Application;
using Whofax.Api.Configuration;
using Whofax.Infrastructure;
using Whofax.Api.Resources;

namespace Whofax.Api;

public class Startup
{
    public IWebHostEnvironment Environment { get; }

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Environment = environment;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddIdentity();
        services.AddAuthentication(Configuration);
        services.AddAuthorizationWithDefaultPolicy();

        services.AddSwaggerDocumentation();

        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers(options =>
        {
            options.Filters.Add(new ApiExceptionFilterAttribute());
        }).AddDataAnnotationsLocalization(options => 
        {
            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(DataAnnotationMessages));
        });

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseSwaggerDocumentation();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
