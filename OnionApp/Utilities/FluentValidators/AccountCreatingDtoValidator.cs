using AppInfrastructure.Database.Configurations;
using AppService.Dtos.Accounts;
using FluentValidation;

namespace OnionApp.Utilities.FluentValidators
{
    public class AccountCreatingDtoValidator : AbstractValidator<AccountCreatingDto>
    {
        public AccountCreatingDtoValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.Email)
                .NotEmpty()
                .MinimumLength(AccountConfiguration.MIN_ACCOUNT_EMAIL_LEGNTH)
                .MaximumLength(AccountConfiguration.MAX_ACCOUNT_EMAIL_LEGNTH)
                .EmailAddress();

            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(AccountConfiguration.MIN_ACCOUNT_PASSWORD_LEGNTH)
                .MaximumLength(AccountConfiguration.MAX_ACCOUNT_PASSWORD_LEGNTH);

            RuleFor(a => a.FirstName)
                .NotEmpty()
                .MinimumLength(UserConfiguration.MIN_USER_FIRSTNAME_LENGTH)
                .MaximumLength(UserConfiguration.MAX_USER_FIRSTNAME_LENGTH);

            When(a => !string.IsNullOrWhiteSpace(a.LastName), () =>
            {
                RuleFor(a => a.LastName)
                    .MinimumLength(UserConfiguration.MIN_USER_LASTNAME_LENGTH)
                    .MaximumLength(UserConfiguration.MAX_USER_LASTNAME_LENGTH);
            });

            When(a => a.DateOfBirth.HasValue, () =>
            {
                RuleFor(a => a.DateOfBirth)
                    .LessThan(DateTime.Now)
                    .GreaterThan(DateTime.Now.AddYears(-1 * UserConfiguration.MAX_NEW_USER_AGE));
            });
        }
    }
}
