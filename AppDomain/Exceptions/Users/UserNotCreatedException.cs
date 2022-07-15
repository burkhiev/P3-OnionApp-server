using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class UserNotCreatedException : UnknownException
    {
        public UserNotCreatedException()
            : base($"User entity was not been created.")
        {

        }
    }
}
