using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Whofax.Domain.Entities.Identity;
using Whofax.Infrastructure.Persistence;

namespace Whofax.Api.Configuration;

public static class IdentityConfiguration
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options =>
            {
                options.Stores.ProtectPersonalData = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddSignInManager()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequiredUniqueChars = 0;

            options.User.RequireUniqueEmail = true;
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationWithDefaultPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.DefaultPolicy = AuthorizationPolicy.Combine(defaultAuthorizationPolicy);
        });

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.SlidingExpiration = true;
                options.Cookie.Name = IdentityConstants.ApplicationScheme;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = context =>
                    {
                        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    },
                };
            });

        return services;
    }
}
