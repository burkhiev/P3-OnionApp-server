using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class AccountNotUpdatedException : UnknownException
    {
        public AccountNotUpdatedException(Guid userId)
            : base($"Database was unable to save account changes. User ID: {userId}.")
        {

        }
    }
}
