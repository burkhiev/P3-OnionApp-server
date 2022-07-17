using AppDomain.Repositories;
using AppService.Abstractions;

namespace AppService.Services
{
    public sealed class BusinessServiceManager : IBusinessServiceManager, IUnitOfWork
    {
        private readonly Lazy<IUserService> _lazyUserService;
        private readonly Lazy<IAccountService> _lazyAccountService;
        private readonly IRepositoryManager _repositoryManager;
        //private bool disposedValue;

        public BusinessServiceManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _lazyUserService = new Lazy<IUserService>(() => new UserService(_repositoryManager));
            _lazyAccountService = new Lazy<IAccountService>(() => new AccountService(_repositoryManager));
        }

        public IUserService UserService => _lazyUserService.Value;
        public IAccountService AccountService => _lazyAccountService.Value;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _repositoryManager.SaveChangesAsync(cancellationToken);
        }

        //private void Dispose(bool disposing)
        //{
        //    if(!disposedValue)
        //    {
        //        if(disposing)
        //        {
        //            _repositoryManager.Dispose();
        //        }

        //        disposedValue = true;
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(disposing: true);
        //    GC.SuppressFinalize(this);
        //}
    }
}
