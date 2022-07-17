using AppDomain.Repositories;
using AppInfrastructure.Database;
using AppInfrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class InfrastructureLayerExtensions
    {
        public static void AddInfrastructureLayerServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContextPool<RepositoryDbContext>(optionsBuilder =>
            {
                string connectionString = configuration.GetConnectionString("Npsql");
                optionsBuilder.UseNpgsql(connectionString, options => options.UseNodaTime());
            });

            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}
