using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class UserNotUpdatedException : UnknownException
    {
        public UserNotUpdatedException(Guid userId)
            : base($"Database was unable to save user changes. User ID: {userId}.")
        {

        }
    }
}
