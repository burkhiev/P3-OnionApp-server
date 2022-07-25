using AppService.Abstractions;
using AppService.Dtos.Users;
using AppService.Services;
using FluentValidation;
using OnionApp.Utilities.FluentValidators;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class ServiceLayerExtensions
    {
        public static void AddServiceLayerServices(this IServiceCollection services)
        {
            // .AddValidatorsFromAssembly() не рекомендуется (по документации).
            //services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.AddScoped<IBusinessServiceManager, BusinessServiceManager>();
        }
    }
}
