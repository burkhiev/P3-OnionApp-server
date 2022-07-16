using AppDomain.Entities;
using Bogus;

namespace AppInfrastructure.Data
{
    internal static class DataInitializer
    {
        private const int DEFAULT_INITIAL_COUNT = 10;

        private static readonly Faker _faker = new Faker();
        private static IEnumerable<Account> _accounts = Enumerable.Empty<Account>();
        private static IEnumerable<User> _users = Enumerable.Empty<User>();

        static DataInitializer()
        {
            GenerateAccountsAndUsers(DEFAULT_INITIAL_COUNT);
        }

        public static IEnumerable<Account> Accounts => _accounts;
        public static IEnumerable<User> Users => _users;

        public static void GenerateAccountsAndUsers(int count)
        {
            var accounts = new List<Account>(count);
            for(int i = 0; i < count; i++)
            {
                accounts.Add(new Account
                {
                    Id = Guid.NewGuid(),
                    Email = _faker.Internet.Email(),
                    Password = _faker.Internet.Password(),
                    CreatedAt = _faker.Date.Past()
                });
            }


            var users = new List<User>(count);
            for(int i = 0; i < count; i++)
            {
                users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    DateOfBirth = _faker.Date.Past(),
                    AccountId = accounts[i].Id,
                });
            }


            _accounts = accounts.AsReadOnly();
            _users = users.AsReadOnly();
        }
    }
}
