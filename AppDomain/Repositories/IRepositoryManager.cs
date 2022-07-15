namespace AppDomain.Repositories
{
    public interface IRepositoryManager : IUnitOfWork, IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IUsersRepository UsersRepository { get; }
    }
}
