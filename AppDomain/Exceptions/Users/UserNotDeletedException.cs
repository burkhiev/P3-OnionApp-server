using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class UserNotDeletedException : UnknownException
    {
        public UserNotDeletedException(Guid userId)
            : base($"Database was unable to remove user with ID: {userId}.")
        {

        }
    }
}
