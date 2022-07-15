using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Accounts
{
    public class AccountDoesNotBelongToUserException : BadRequestException
    {
        public AccountDoesNotBelongToUserException(Guid userId, Guid accountId)
            :base($"The account with ID {accountId} does bot belong to the user with ID {userId}.")
        {

        }
    }
}
