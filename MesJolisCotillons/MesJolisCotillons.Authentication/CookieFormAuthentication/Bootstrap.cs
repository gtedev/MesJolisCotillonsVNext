namespace MesJolisCotillons.Authentication.CookieFormAuthentication
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Owin.Security.Cookies;
    using System;
    using System.Threading.Tasks;

    public static class Bootstrap
    {
        public static IServiceCollection AddCookieFormAuthentication(this IServiceCollection services, IHostingEnvironment env)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationType)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = env.IsDevelopment() ? TimeSpan.FromMinutes(5) : TimeSpan.FromMinutes(20);
            });

            return services;
        }
    }
}
