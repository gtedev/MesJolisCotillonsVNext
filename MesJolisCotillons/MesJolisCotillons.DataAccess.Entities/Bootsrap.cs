namespace MesJolisCotillons.DataAccess.Entities
{
    using MesJolisCotillons.DataAccess.Entities.Context;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Bootsrap
    {
        public static IServiceCollection AddMesJolisCotillonsDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MesJolisCotillonsDatabase");

            services.AddDbContext<MesJolisCotillonsContext>(options =>
                options.UseSqlServer(connectionString));
            ////.UseLazyLoadingProxies());

            services.AddScoped<IMesJolisCotillonsContext>(p => p.GetRequiredService<MesJolisCotillonsContext>());
            services.AddScoped<IUserDbContext>(p => p.GetRequiredService<MesJolisCotillonsContext>());
            services.AddScoped<ISaveDbContext>(p => p.GetRequiredService<MesJolisCotillonsContext>());

            return services;
        }

    }
}
