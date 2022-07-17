using AppDomain.Exceptions.Abstractions;
using OnionApp.Utilities.ResponseTypes;

namespace OnionApp.Middlewares
{
    internal sealed class CustomExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            int statusCode = ex switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ClientErrorResponse
            {
                Status = statusCode,
                Errors = new List<KeyValuePair<string, string>>()
            };

            response.Errors.Add(new KeyValuePair<string, string>(string.Empty, ex.Message));

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
