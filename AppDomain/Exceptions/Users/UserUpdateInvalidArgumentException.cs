using AppDomain.Exceptions.Abstractions;

namespace AppDomain.Exceptions.Users
{
    public class UserUpdateInvalidArgumentException : BadRequestException
    {
        public UserUpdateInvalidArgumentException(string arg, Guid userId)
            : base($"Users' IDs don't match each other. Arg: {arg}. Value: {userId}")
        {
        }
    }
}
