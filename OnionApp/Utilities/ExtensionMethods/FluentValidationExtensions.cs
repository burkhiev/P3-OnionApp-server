using AppService.Dtos.Users;
using FluentValidation;
using OnionApp.Utilities.FluentValidators;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Регистрирует зависимости для валидации DTO моделей (FluentValidation package).
        /// </summary>
        /// <param name="services">
        /// Specifies the contract for a collection of service descriptors.
        /// </param>
        public static void AddDtoValidationServices(this IServiceCollection services)
        {
            // .AddValidatorsFromAssembly() не рекомендуется (по документации).
            //services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
        }
    }
}
