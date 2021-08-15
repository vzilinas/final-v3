﻿using Whofax.Api.Common.Filters;
using Whofax.Application;

namespace Whofax.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();

        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddControllers(options =>
        {
            options.Filters.Add(new ApiExceptionFilterAttribute());
        });

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
