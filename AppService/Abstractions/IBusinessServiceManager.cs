using AppDomain.Repositories;

namespace AppService.Abstractions
{
    public interface IBusinessServiceManager : IUnitOfWork
    {
        IAccountService AccountService { get; }
        IUserService UserService { get; }
    }
}