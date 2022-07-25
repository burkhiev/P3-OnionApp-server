﻿using AppService.Dtos.Users;
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
            var respose = new ClientErrorResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<KeyValuePair<string, string>>()
            };

            foreach(var pair in context.ActionArguments)
            {
                if(pair.Value is UserDto userDto)
                {
                    var result =await _validator.ValidateAsync(userDto);

                    if(result.IsValid)
                    {
                        continue; 
                    }

                    foreach(var error in result.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        respose.Errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
                    }
                }
            }

            if(respose.Errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(respose);
                return;
            }

            await next();
        }
    }
}
