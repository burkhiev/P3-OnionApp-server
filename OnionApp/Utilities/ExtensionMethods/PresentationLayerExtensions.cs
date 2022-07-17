using AppService.Dtos.Users;
using FluentValidation;
using Microsoft.OpenApi.Models;
using OnionApp.Middlewares;
using OnionApp.Utilities.FluentValidators;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class PresentationLayerExtensions
    {
        public static void AddPresentationLayerServices(this IServiceCollection services)
        {
            string asmName = typeof(Program).Assembly.GetName().Name!;

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
                options.SwaggerDoc(asmName, new OpenApiInfo { Title = asmName }));

            services.AddSingleton<CustomExceptionHandlerMiddleware>();
            services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
