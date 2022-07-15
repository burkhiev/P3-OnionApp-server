using AppDomain.Entities;
using AppInfrastructure.Database;
using AppInfrastructure.Database.Repositories;
using AppService.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace OnionApp.Tests.Data
{
    public class ServiceManagerForUsersFixture : IDisposable
    {
        private const int FAKE_USERS_COUNT = 10;
        private const string FAKE_DB_NAME = "TEST_DB";

        private readonly RepositoryDbContext _db;
        private readonly RepositoryManager _repositoryManager;
        private readonly BusinessServiceManager _serviceManager;

        public ServiceManagerForUsersFixture()
        {
            var options = new DbContextOptionsBuilder<RepositoryDbContext>()
                .UseInMemoryDatabase(FAKE_DB_NAME)
                .Options;

            _db = new RepositoryDbContext(options);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            _repositoryManager = new RepositoryManager(_db);
            _serviceManager = new BusinessServiceManager(_repositoryManager);
        }

        public RepositoryDbContext DbContext => _db;
        public BusinessServiceManager ServiceManager => _serviceManager;
        private Faker<User> UsersFabric
        { 
            get => new Faker<User>("ru")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.DateOfBirth, f => f.Date.Past());
        }

        public void Initialize()
        {
            var users = new List<User>(FAKE_USERS_COUNT);

            for(int i = 0; i < FAKE_USERS_COUNT; i++)
            {
                users.Add(UsersFabric.Generate());
            }

            _db.Users.AddRange(users);
            _db.SaveChanges();
        }

        public void Reset()
        {
            var users = _db.Users;
            _db.Users.RemoveRange(users);
        }

        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
            _repositoryManager.Dispose();
            _serviceManager.Dispose();
        }
    }
}
