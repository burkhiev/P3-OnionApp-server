using AppDomain.Repositories;

namespace AppInfrastructure.Database.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _db;
        private readonly IAccountRepository _accountRepository;
        private readonly IUsersRepository _usersRepository;

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
    }
}
