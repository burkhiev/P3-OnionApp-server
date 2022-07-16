using AppDomain.Entities;
using AppInfrastructure.Database;
using AppInfrastructure.Database.Repositories;
using AppService.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnionApp.Tests.Data
{
    public class ServiceManagerFixture : IDisposable
    {
        public const int FAKE_ACCOUNTS_COUNT = 10;

        private readonly string FAKE_DB_NAME = Guid.NewGuid().ToString();
        private readonly RepositoryDbContext _db;
        private readonly RepositoryManager _repositoryManager;
        private readonly BusinessServiceManager _serviceManager;

        public ServiceManagerFixture()
        {
            var options = new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(FAKE_DB_NAME)
                .Options;

            _db = new RepositoryDbContext(options);
            _repositoryManager = new RepositoryManager(_db);
            _serviceManager = new BusinessServiceManager(_repositoryManager);
        }

        private Faker<User> UsersFabric
        {
            get => new Faker<User>("ru")
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past());
        }

        private Faker<Account> AccountFabric
        {
            get => new Faker<Account>("ru")
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.Email, f => f.Internet.Email())
                .RuleFor(a => a.Password, f => f.Internet.Password())
                .RuleFor(a => a.CreatedAt, f => f.Date.Past());
        }

        public RepositoryDbContext DbContext => _db;
        public BusinessServiceManager ServiceManager => _serviceManager;

        public void Initialize()
        {
            _db.Accounts.Load();
            _db.Accounts.RemoveRange(_db.Accounts.ToList());
            _db.SaveChanges();


            var accounts = new List<Account>(FAKE_ACCOUNTS_COUNT);

            for(int i = 0; i < FAKE_ACCOUNTS_COUNT; i++)
            {
                accounts.Add(AccountFabric.Generate());
            }


            var users = new List<User>(FAKE_ACCOUNTS_COUNT);

            for(int i = 0; i < FAKE_ACCOUNTS_COUNT; i++)
            {
                var user = UsersFabric.Generate();
                user.AccountId = accounts[i].Id;
                users.Add(user);
            }
            

            _db.Accounts.AddRange(accounts);
            _db.Users.AddRange(users);
            _db.SaveChanges();
        }

        public void Reset()
        {
            _db.Accounts.Load();
            _db.Accounts.RemoveRange(_db.Accounts.ToList());
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
            _serviceManager.Dispose();
        }
    }
}
