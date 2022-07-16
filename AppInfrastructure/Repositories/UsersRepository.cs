using AppDomain.Entities;
using AppDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppInfrastructure.Database.Repositories
{
    internal sealed class UsersRepository : IUsersRepository
    {
        private const int MAX_ID_SEARCH_COUNT = 5;
        private readonly RepositoryDbContext _db;

        public UsersRepository(RepositoryDbContext db) => _db = db;

        public IQueryable<User> GetAll()
        {
            var users = _db.Users.AsQueryable();
            return users;
        }

        public async Task<User?> FindByIdAsync(Guid accountId, CancellationToken ct = default)
        {
            var user = await _db.Users.FindAsync(new object[] { accountId }, ct);
            return user;
        }

        public async Task<User> AddAsync(User user, CancellationToken ct = default)
        {
            var newUserId = Guid.NewGuid();

            for(int i = 0; i < MAX_ID_SEARCH_COUNT; i++)
            {
                var result = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id, ct);

                if(result is null)
                {
                    break;
                }

                newUserId = Guid.NewGuid();
            }

            user.Id = newUserId;

            var userEntry = await _db.Users.AddAsync(user, ct);
            return userEntry.Entity;
        }


        public User Update(User user)
        {
            var newUserEntry = _db.Users.Update(user);
            return newUserEntry.Entity;
        }

        public User Remove(User user)
        {
            var removedUserEntry = _db.Users.Remove(user);
            return removedUserEntry.Entity;
        }

        public IQueryable<User> FindByCondition(Expression<Func<User, bool>> condition, CancellationToken cancellationToken = default)
        {
            var users = _db.Users.Where(condition);
            return users;
        }

        public void RemoveRange(User[] entities)
        {
            _db.Users.RemoveRange(entities);
        }
    }
}
