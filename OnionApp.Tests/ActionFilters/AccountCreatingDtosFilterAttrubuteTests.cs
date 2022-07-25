using AppService.Dtos.Accounts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using OnionApp.Filters;
using OnionApp.Utilities.FluentValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Routing;
using OnionApp.Utilities.ResponseTypes;
using Bogus;
using OnionApp.Tests.Utilities;
using AppInfrastructure.Database.Configurations;

namespace OnionApp.Tests.ActionFilters
{
    public class AccountCreatingDtosFilterAttrubuteTests
    {
        private readonly Faker _faker = new Faker("ru");
        private readonly AccountCreatingDtosFilterAttrubute _filter;
        private readonly AccountCreatingDto _validDto;
        private readonly ActionExecutionDelegate _actionExecutionDelegate;

        public AccountCreatingDtosFilterAttrubuteTests()
        {
            _filter = new AccountCreatingDtosFilterAttrubute(
                validator: new AccountCreatingDtoValidator());

            _validDto = new AccountCreatingDto
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password(),
                DateOfBirth = _faker.Date.Past(20)
            };

            _actionExecutionDelegate = (new Mock<ActionExecutionDelegate>()).Object;
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter doesn't catch any validation errors when arguments are valid")]
        public async Task AccountCreatingDtosFilter_DoesntCatchAnyValidationErrors_WhenArgumentsAreValid()
        {
            var actionExecutingContext = GetActionExecutingContext(_validDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            Assert.Null(actionExecutingContext.Result);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when email is too long")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenEmailIsTooLong()
        {
            var invalidDto = _validDto;
            invalidDto.Email = _faker.GetStringWithLength(AccountConfiguration.MAX_ACCOUNT_EMAIL_LEGNTH + 10);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.Email);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when email is short")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenEmailIsShort()
        {
            var invalidDto = _validDto;
            invalidDto.Email = _faker.GetStringWithLength(AccountConfiguration.MIN_ACCOUNT_EMAIL_LEGNTH - 1);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(_validDto.Email);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when password is too long")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenPasswordIsTooLong()
        {
            var invalidDto = _validDto;
            invalidDto.Password = _faker.GetStringWithLength(AccountConfiguration.MAX_ACCOUNT_PASSWORD_LEGNTH + 10);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.Password);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when password is short")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenPasswordIsShort()
        {
            var invalidDto = _validDto;
            invalidDto.Password = _faker.GetStringWithLength(AccountConfiguration.MIN_ACCOUNT_PASSWORD_LEGNTH - 1);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.Password);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when firstname is too long")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenFirstNameIsTooLong()
        {
            var invalidDto = _validDto;
            invalidDto.FirstName = _faker.GetStringWithLength(UserConfiguration.MAX_USER_FIRSTNAME_LENGTH + 10);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.FirstName);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when firstname is short")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenFirstNameIsShort()
        {
            var invalidDto = _validDto;
            invalidDto.FirstName = _faker.GetStringWithLength(UserConfiguration.MIN_USER_FIRSTNAME_LENGTH - 1);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.FirstName);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when lastname is too long")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenLastNameIsTooLong()
        {
            var invalidDto = _validDto;
            invalidDto.LastName = _faker.GetStringWithLength(UserConfiguration.MAX_USER_LASTNAME_LENGTH + 10);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.LastName);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns bad request result with error data when lastname is short")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithErrorData_WhenLastNameIsShort()
        {
            var invalidDto = _validDto;
            invalidDto.LastName = _faker.GetStringWithLength(UserConfiguration.MIN_USER_LASTNAME_LENGTH - 1);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.LastName);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter has no errors when lastname is null")]
        public async Task AccountCreatingDtosFilter_HasNoErrors_WhenLastNameIsNull()
        {
            _validDto.LastName = null;
            var actionExecutingContext = GetActionExecutingContext(_validDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            Assert.Null(actionExecutingContext.Result);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter has no errors when lastname is empty string")]
        public async Task AccountCreatingDtosFilter_HasNoErrors_WhenLastNameIsEmptyString()
        {
            _validDto.LastName = string.Empty;
            var actionExecutingContext = GetActionExecutingContext(_validDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            Assert.Null(actionExecutingContext.Result);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns BadRequestResult with error when birth date is too far back")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithError_WhenBirthDateIsTooFarBack()
        {
            var invalidDto = _validDto;
            invalidDto.DateOfBirth = DateTime.MinValue;
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.DateOfBirth);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter returns BadRequestResult with error when birth date is in future")]
        public async Task AccountCreatingDtosFilter_ReturnsBadRequestResult_WithError_WhenBirthDateIsInFuture()
        {
            var invalidDto = _validDto;
            invalidDto.DateOfBirth = DateTime.Now.AddDays(1);
            var actionExecutingContext = GetActionExecutingContext(invalidDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            var error = GetActionExecutingResultError(actionExecutingContext.Result);
            Assert.NotNull(error);
            string propertyName = nameof(invalidDto.DateOfBirth);
            Assert.Equal(propertyName, error.Key);
        }

        [Fact(DisplayName = "AccountCreatingDtosFilter has no error when birth date is null")]
        public async Task AccountCreatingDtosFilter_HasNoError_WhenBirthDateIsNull()
        {
            _validDto.DateOfBirth = null;
            var actionExecutingContext = GetActionExecutingContext(_validDto);

            await _filter.OnActionExecutionAsync(actionExecutingContext, _actionExecutionDelegate);

            Assert.Null(actionExecutingContext.Result);
        }

        private KeyValuePair<string, string> GetActionExecutingResultError(IActionResult? actionResult)
        {
            var result = Assert.IsType<BadRequestObjectResult>(actionResult);
            var responseObj = Assert.IsType<ClientErrorResponse>(result.Value);

            Assert.Equal(StatusCodes.Status400BadRequest, responseObj.Status);
            var error = Assert.Single(responseObj.Errors);

            return error;
        }

        private ActionExecutingContext GetActionExecutingContext(AccountCreatingDto accountCreatingDto)
        {

            var actionArguments = new Dictionary<string, object?> {{ nameof(accountCreatingDto), accountCreatingDto }};
            var httpContext = new DefaultHttpContext();

            // Несмотря на то, что у ActionContext есть 2 конструктора с количеством
            // аргументов 0 и 1, они выбрасывают исключение.
            // А именно: нужны все 3 параметра перечисленные ниже.
            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ActionDescriptor());

            var actionExecutingContext = new ActionExecutingContext(actionContext,
                filters: new List<IFilterMetadata>(),
                actionArguments: actionArguments,
                controller: new object());

            return actionExecutingContext;
        }
    }
}
