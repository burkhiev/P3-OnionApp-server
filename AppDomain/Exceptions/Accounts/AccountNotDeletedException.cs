using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountNotDeletedException : UnknownException
    {
        public AccountNotDeletedException(Guid userId)
            : base($"Database was unable to remove account with ID: {userId}.")
        {

        }
    }
}
