using AppDomain.Repositories;

namespace AppInfrastructure.Database.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _db;
        private readonly IAccountRepository _accountRepository;
        private readonly IUsersRepository _usersRepository;
        private bool disposedValue;

        public RepositoryManager(RepositoryDbContext db)
        {
            _db = db;
            _accountRepository = new AccountsRepository(_db);
            _usersRepository = new UsersRepository(_db);
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public IUsersRepository UsersRepository => _usersRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _db.SaveChangesAsync(cancellationToken);

        private void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    // Очищение DbContext нивелирует плюсы полученные от использования пула DbContext.
                    // _db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
