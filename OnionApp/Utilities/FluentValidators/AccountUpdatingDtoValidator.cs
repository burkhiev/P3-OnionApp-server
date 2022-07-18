using AppInfrastructure.Database.Configurations;
using AppService.Dtos.Accounts;
using FluentValidation;

namespace OnionApp.Utilities.FluentValidators
{
    public class AccountUpdatingDtoValidator : AbstractValidator<AccountUpdatingDto>
    {
        public AccountUpdatingDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.Email)
                .NotEmpty()
                .MinimumLength(AccountConfiguration.MIN_ACCOUNT_EMAIL_LEGNTH)
                .MaximumLength(AccountConfiguration.MAX_ACCOUNT_EMAIL_LEGNTH);

            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(AccountConfiguration.MIN_ACCOUNT_PASSWORD_LEGNTH)
                .MaximumLength(AccountConfiguration.MAX_ACCOUNT_PASSWORD_LEGNTH);
        }
    }
}
