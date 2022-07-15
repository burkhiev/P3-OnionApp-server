using OnionApp.Middlewares;

namespace OnionApp.Utilities.ExtensionMethods
{
    public static class ExceptionHandlingExtensions
    {
        public static void AddExceptionHandlingServices(this IServiceCollection services)
        {
            services.AddSingleton<ExceptionHandlerMiddleware>();
        }

        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
