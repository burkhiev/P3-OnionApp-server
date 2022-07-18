using AppService.Dtos.Accounts;
using AppService.Dtos.Users;
using FluentValidation;
using Microsoft.OpenApi.Models;
using OnionApp.Middlewares;
using OnionApp.Utilities.FluentValidators;
using System.Reflection;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class PresentationLayerExtensions
    {
        public static void AddPresentationLayerServices(this IServiceCollection services)
        {
            string asmName = typeof(Program).Assembly.GetName().Name!;
            string version = "v1";

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = asmName,
                        Version = version
                    });

                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                });


            services.AddSingleton<CustomExceptionHandlerMiddleware>();
            services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
            services.AddTransient<IValidator<AccountCreatingDto>, AccountCreatingDtoValidator>();
            services.AddTransient<IValidator<AccountUpdatingDto>, AccountUpdatingDtoValidator>();
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
