using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountNotCreatedException : UnknownException
    {
        public AccountNotCreatedException()
            : base($"Account entity was not been created.")
        {

        }
    }
}
