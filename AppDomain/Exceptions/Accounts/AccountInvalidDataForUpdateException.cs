using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountInvalidDataForUpdateException : UnknownException
    {
        public AccountInvalidDataForUpdateException(Guid acountId) 
            : base($"Invalid data while updating user(ID: {acountId}) received.")
        {
        }
    }
}
