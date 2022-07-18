using AppInfrastructure.Database.Configurations;
using AppService.Dtos.Accounts;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using OnionApp.Tests.Exceptions;
using OnionApp.Tests.Utilities;
using OnionApp.Utilities.FluentValidators;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace OnionApp.Tests.Validators
{
    public class AccountCreatingDtoValidatorTests
    {
        private readonly Faker _faker = new Faker("ru");
        private readonly AccountCreatingDto _validDto;
        private readonly DateTime _validDateOfBirth = DateTime.UtcNow.AddYears(-10);

        private readonly IValidator<AccountCreatingDto> _validator;

        public AccountCreatingDtoValidatorTests() 
        {
            _validDto = new AccountCreatingDto
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password(),
                DateOfBirth = _faker.Date.Past(20)
            };

            _validator = new AccountCreatingDtoValidator();
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator doesn't return errors")]
        public void AccountCreatingDtoValidator_DoesntReturnErrors()
        {
            var result = _validator.TestValidate(_validDto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when email is too large")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenEmailIsTooLarge()
        {
            _validDto.Email = _faker.GetStringWithLength(AccountConfiguration.MAX_ACCOUNT_EMAIL_LEGNTH + 10);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.Email);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when email is short")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenEmailIsShort()
        {
            _validDto.Email = _faker.GetStringWithLength(AccountConfiguration.MIN_ACCOUNT_EMAIL_LEGNTH - 1);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.Email);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when password is too large")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenPasswordIsTooLarge()
        {
            _validDto.Password = _faker.GetStringWithLength(AccountConfiguration.MAX_ACCOUNT_PASSWORD_LEGNTH + 10);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.Password);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when password is short")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenPasswordIsShort()
        {
            _validDto.Password = _faker.GetStringWithLength(AccountConfiguration.MIN_ACCOUNT_PASSWORD_LEGNTH - 1);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.Password);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when firstname is too large")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenFirstNameIsTooLarge()
        {
            _validDto.FirstName = _faker.GetStringWithLength(UserConfiguration.MAX_USER_FIRSTNAME_LENGTH + 10);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.FirstName);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when fisrtname is short")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenFirstNameIsShort()
        {
            _validDto.FirstName = _faker.GetStringWithLength(UserConfiguration.MIN_USER_FIRSTNAME_LENGTH - 1);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.FirstName);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when lastname is too large")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenLastNameIsTooLarge()
        {
            
            _validDto.LastName = _faker.GetStringWithLength(UserConfiguration.MAX_USER_LASTNAME_LENGTH + 10);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.LastName);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has error when lastname is short")]
        public void AccountCreatingDtoValidator_ReturnsError_WhenLastNameIsShort()
        {
            
            _validDto.LastName = _faker.GetStringWithLength(UserConfiguration.MIN_USER_LASTNAME_LENGTH - 1);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.LastName);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator hasn't errors when lastname is empty string")]
        public void AccountCreatingDtoValidator_DoesntReturnError_WhenLastNameIsEmptyString()
        {
            _validDto.LastName = string.Empty;

            var result = _validator.TestValidate(_validDto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator hasn't errors when lastname is null")]
        public void AccountCreatingDtoValidator_DoesntReturnError_WhenLastNameIsNull()
        {
            
            _validDto.LastName = null;

            var result = _validator.TestValidate(_validDto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has errors when date of birth is invalid")]
        public void AccountCreatingDtoValidator_HasErrors_WhenDateOfBirthIsTooFarBack()
        {
            
            _validDto.DateOfBirth = DateTime.MinValue;

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.DateOfBirth);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator has errors when date of birth is invalid")]
        public void AccountCreatingDtoValidator_HasErrors_WhenDateOfBirthIsFromFuture()
        {
            
            _validDto.DateOfBirth = DateTime.Now.AddDays(1);

            var result = _validator.TestValidate(_validDto);
            result.ShouldHaveValidationErrorFor(dto => dto.DateOfBirth);
        }

        [Fact(DisplayName = "AccountCreatingDtoValidator hasn't any errors when date of birth is null")]
        public void AccountCreatingDtoValidator_HasntAnyErrors_WhenDateOfBirthIsNull()
        {
            
            _validDto.DateOfBirth = null;

            var result = _validator.TestValidate(_validDto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
