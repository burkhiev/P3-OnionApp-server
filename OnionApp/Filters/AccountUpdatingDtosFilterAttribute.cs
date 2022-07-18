using AppService.Dtos.Accounts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnionApp.Utilities.ResponseTypes;

namespace OnionApp.Filters
{
    public class AccountUpdatingDtosFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IValidator<AccountUpdatingDto> _validator;

        public AccountUpdatingDtosFilterAttribute(IValidator<AccountUpdatingDto> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var response = new ClientErrorResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<KeyValuePair<string, string>>()
            };

            foreach(var arg in context.ActionArguments)
            {
                if(arg.Value is AccountUpdatingDto accountDto)
                {
                    var result = await _validator.ValidateAsync(accountDto);

                    if(result.IsValid)
                    {
                        continue;
                    }

                    foreach(var error in result.Errors)
                    {
                        response.Errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
                    }
                }
            }

            if(response.Errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(response);
                return;
            }

            await next();
        }
    }
}
