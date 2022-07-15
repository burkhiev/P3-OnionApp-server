using AppDomain.Repositories;

namespace AppService.Abstractions
{
    public interface IBusinessServiceManager : IUnitOfWork, IDisposable
    {
        IAccountService AccountService { get; }
        IUserService UserService { get; }
    }
}