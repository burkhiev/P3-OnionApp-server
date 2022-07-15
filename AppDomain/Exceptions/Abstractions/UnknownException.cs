namespace AppDomain.Exceptions.Abstractions
{
    public abstract class UnknownException : Exception
    {
        protected UnknownException(string message)
            : base(message)
        {

        }
    }
}
