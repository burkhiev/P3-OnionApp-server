using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException(Guid accountId)
            : base($"Account with ID {accountId} was not found.")
        {
        }
    }
}
