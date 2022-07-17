using AppService.Dtos.Users;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnionApp.Utilities.ResponseTypes;

namespace OnionApp.Filters
{
    public class UserDtosValidationFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IValidator<UserDto> _validator;

        public UserDtosValidationFilterAttribute(IValidator<UserDto> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach(var pair in context.ActionArguments)
            {
                if(pair.Value is UserDto userDto)
                {
                    var result = _validator.Validate(userDto);

                    if(!result.IsValid)
                    {
                        var respose = new ClientErrorResponse
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Errors = new List<KeyValuePair<string, string>>(result.Errors.Count)
                        };

                        foreach(var error in result.Errors)
                        {
                            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                            respose.Errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
                        }

                        context.Result = new BadRequestObjectResult(respose);
                        return;
                    }
                }
            }

            await next();
        }
    }
}
