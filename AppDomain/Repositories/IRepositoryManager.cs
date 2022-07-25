namespace AppDomain.Repositories
{
    public interface IRepositoryManager : IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IUsersRepository UsersRepository { get; }
    }
}
