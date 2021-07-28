namespace MesJolisCotillons.VNext
{
    using System;
    using System.IO;
    using System.Reflection;
    using Autofac;
    using AutoMapper;
    using MesJolisCotillons.Adapters;
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Core;
    using MesJolisCotillons.DataAccess.Entities;
    using MesJolisCotillons.DataAccess.Repositories;
    using MesJolisCotillons.DataAccess.Repositories.AutoMapper;
    using MesJolisCotillons.Executors;
    using MesJolisCotillons.Operations;
    using MesJolisCotillons.Validation;
    using MesJolisCotillons.VNext.DI.Autofac.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            });

            services.AddMesJolisCotillonsDbContext(this.Configuration)
                .AddMemoryCache()
                .AddAutoMapper(typeof(RepositoriesProfile))
                .AddHttpContextAccessor()
                .AddSession(options =>
                {
                    // Set a short timeout for easy testing.
                    options.IdleTimeout = TimeSpan.FromSeconds(10);
                    options.Cookie.HttpOnly = true;
                })
                .AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "App/BackOffice/angular-admin/dist";
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Mes Jolis Cotillons API",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterResources()
                   .RegisterRepositories()
                   .RegisterAdapters()
                   .RegisterCommands()
                   .RegisterValidations()
                   .RegisterExecutors()
                   .RegisterResponse()
                   .RegisterOperations()
                   .RegisterValidationServices()
                   .RegisterOperationsServices()
                   .RegisterServices()
                   .RegisterCore()
                   .RegisterOperationsServices();
        }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            this.CurrentEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
              {
                  routes.MapRoute(
                      name: "default",
                      template: "{controller=Home}/{action=Index}/{id?}");
              });

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api/docs/swagger";
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "App/BackOffice/angular-admin";
                spa.Options.DefaultPage = "/Admin";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
