using AppDomain.Entities;
using AppDomain.Repositories;
using System.Linq.Expressions;

namespace AppInfrastructure.Database.Repositories
{
    internal sealed class AccountsRepository : IAccountRepository
    {
        private readonly RepositoryDbContext _db;

        public AccountsRepository(RepositoryDbContext db) => _db = db;

        public IQueryable<Account> GetAll()
        {
            var accounts = _db.Accounts.AsQueryable();
            return accounts;
        }

        public async Task<Account?> FindByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            var account = await _db.Accounts.FindAsync(new object[] { accountId }, cancellationToken);
            return account;
        }

        public IQueryable<Account> FindByCondition(Expression<Func<Account, bool>> condition, CancellationToken cancellationToken = default)
        {
            var accounts = _db.Accounts.Where(condition);
            return accounts;
        }

        public async Task<Account> AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            var accountEntry = await _db.AddAsync(account, cancellationToken);
            return accountEntry.Entity;
        }

        public Account Update(Account account)
        {
            var newAccountEntry = _db.Update(account);
            return newAccountEntry.Entity;
        }

        public Account Remove(Account account)
        {
            var removedAccountEntry = _db.Remove(account);
            return removedAccountEntry.Entity;
        }

        public void RemoveRange(Account[] entities)
        {
            _db.RemoveRange(entities);
        }
    }
}
