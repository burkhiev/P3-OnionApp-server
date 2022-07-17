using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountInvalidArgumentForUpdateException : UnknownException
    {
        public AccountInvalidArgumentForUpdateException(Guid acountId)
            : base($"Invalid argument while updating user received. User ID: {acountId}")
        {
        }
    }
}
