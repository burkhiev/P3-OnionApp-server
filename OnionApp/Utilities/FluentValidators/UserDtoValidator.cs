using AppService.Dtos.Users;
using FluentValidation;
using AppInfrastructure.Database.Configurations;

namespace OnionApp.Utilities.FluentValidators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(user => user.Id).NotEmpty();

            RuleFor(user => user.FirstName)
                .NotEmpty()
                .MinimumLength(UserConfiguration.MIN_USER_FIRSTNAME_LENGTH)
                .MaximumLength(UserConfiguration.MAX_USER_FIRSTNAME_LENGTH);
        }
    }
}
