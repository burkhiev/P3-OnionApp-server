using AppDomain.Repositories;
using AppService.Abstractions;
using AppService.Services;
using AppInfrastructure.Database.Repositories;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class RepositoriesExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessServiceManager, BusinessServiceManager>();
        }
    }
}
