using Microsoft.AspNetCore.Authentication.Cookies;
using ChronoFlow.API.Modules.UserModule;

namespace ChronoFlow.API.Infrastructure
{
    public class AuthModule : IModule
    {
        public void ConfigureHubs(WebApplication app)
        {
        }

        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = (context) =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorization();
            services.AddScoped<PasswordHasher>();
            return services;
        }
    }
}