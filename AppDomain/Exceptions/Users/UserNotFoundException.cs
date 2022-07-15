using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(Guid userId)
            : base($"User with ID {userId} was not found.")
        {
        }
    }
}
