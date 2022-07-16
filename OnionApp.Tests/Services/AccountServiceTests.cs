using AppDomain.Entities;
using AppDomain.Exceptions.Accounts;
using AppInfrastructure.Database;
using AppService.Dtos.Accounts;
using AppService.Services;
using Bogus;
using OnionApp.Tests.Data;
using OnionApp.Tests.Exceptions;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace OnionApp.Tests.Services
{
    public class AccountServiceTests : IClassFixture<ServiceManagerFixture>, IDisposable
    {
        private readonly ServiceManagerFixture _fixture;
        private readonly Faker _faker = new Faker("ru");

        public AccountServiceTests(ServiceManagerFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize();

            if(_fixture.DbContext.Users.Count() != ServiceManagerFixture.FAKE_ACCOUNTS_COUNT)
            {
                throw new InvalidFixtureDataException();
            }

            if(_fixture.DbContext.Accounts.Count() != ServiceManagerFixture.FAKE_ACCOUNTS_COUNT)
            {
                throw new InvalidFixtureDataException();
            }
        }

        public void Dispose()
        {
            _fixture.Reset();
        }

        public RepositoryDbContext Db => _fixture.DbContext;
        public BusinessServiceManager ServiceManager => _fixture.ServiceManager;

        [Fact(DisplayName = "GetAllAsync should return all accounts")]
        public async Task GetAllAsync_should_return_all_accounts()
        {
            var accountsDtos = await ServiceManager.AccountService.GetAllAsync();
            var accounts = Db.Accounts.ToList();

            var temp = accountsDtos.ToArray();

            Assert.Equal(accounts.Count(), accountsDtos.Count());
            Assert.All(accountsDtos, dto => Assert.Contains(accounts, a => a.Id == dto.Id));
        }

        [Fact(DisplayName = "GetAllAsync should return empty accounts collection")]
        public async Task GetAllAsync_should_return_empty_accounts_and_users_collections()
        {
            Db.Accounts.RemoveRange(Db.Accounts);
            Db.SaveChanges();

            Assert.Equal(0, Db.Accounts.Count());
            Assert.Equal(0, Db.Users.Count());

            var accountsDtos = await ServiceManager.AccountService.GetAllAsync();
            var usersDtos = await ServiceManager.UserService.GetAllAsync();

            Assert.Empty(accountsDtos);
            Assert.Empty(usersDtos);
        }

        [Fact(DisplayName = "FindByIdAsync should return founded account dto")]
        public async Task FindByIdAsync_should_return_founded_account_dto()
        {
            var accountsIds = (await ServiceManager.AccountService.GetAllAsync()).Select(a => a.Id).ToArray();
            int accountIndex = _faker.Random.Int(0, accountsIds.Length - 1);
            Guid accountId = accountsIds[accountIndex];

            var accountFromDb = await Db.Accounts.FindAsync(accountId);
            var accountFromService = await ServiceManager.AccountService.FindByIdAsync(accountId);

            Assert.NotNull(accountFromDb);
            Assert.NotNull(accountFromService);
            Assert.Equal(accountFromDb!.Id, accountFromService!.Id);
        }

        [Fact(DisplayName = "FindByIdAsync should return null if user with specified id is not exist")]
        public async Task FindByIdAsync_should_return_null_if_user_with_specified_id_is_not_exist()
        {
            var accountsIds = (await ServiceManager.AccountService.GetAllAsync())
                .Select(a => a.Id)
                .ToList();

            Guid accountId = Guid.NewGuid();
            while(accountsIds.Contains(accountId))
            {
                accountId = Guid.NewGuid();
            }

            var accountDto = await ServiceManager.AccountService.FindByIdAsync(accountId);

            Assert.Null(accountDto);
        }

        [Fact(DisplayName = "UpdateAsync should update email and password")]
        public async Task UpdateAsync_should_update_email_and_password()
        {
            var accountsIds = (await ServiceManager.AccountService.GetAllAsync())
                .Select(a => a.Id)
                .ToArray();

            int accountIndex = _faker.Random.Int(0, accountsIds.Length - 1);
            var accountId = accountsIds[accountIndex];

            var oldAccount = await ServiceManager.AccountService.FindByIdFullWithoutIncludes(accountId);

            string newEmail = _faker.Internet.Email();
            string newPassword= _faker.Internet.Password();

            while(newEmail == oldAccount.Email || newPassword == oldAccount.Password)
            {
                newEmail = _faker.Internet.Email();
                newPassword = _faker.Internet.Password();
            }

            var accountUpdatingDto = new AccountUpdatingDto
            {
                AccountId = oldAccount.Id,
                Email = newEmail,
                Password = newPassword
            };

            await ServiceManager.AccountService.UpdateAsync(accountUpdatingDto.AccountId, accountUpdatingDto);
            await ServiceManager.SaveChangesAsync();

            var newAccount= await ServiceManager.AccountService.FindByIdFullWithoutIncludes(accountId);

            Assert.NotNull(newAccount);
            Assert.Equal(oldAccount.Id, newAccount.Id);
            Assert.Equal(newEmail, newAccount.Email);
            Assert.Equal(newPassword, newAccount.Password);
            Assert.NotEqual(oldAccount.Email, newAccount.Email);
            Assert.NotEqual(oldAccount.Password, newAccount.Password);
        }

        [Fact(DisplayName = "UpdateAsync throws account not found error when if account ID is invalid")]
        public async Task UpdateAsync_throws_account_not_found_error_when_if_account_ID_is_invalid()
        {
            var accountIds = (await ServiceManager.AccountService.GetAllAsync())
                .Select(x => x.Id)
                .ToArray();

            Guid accountId = Guid.NewGuid();
            while(accountIds.Contains(accountId))
            {
                accountId = Guid.NewGuid();
            }

            var accountUpdatingDto = new AccountUpdatingDto { AccountId = accountId };

            await Assert.ThrowsAsync<AccountNotFoundException>(async () => 
                await ServiceManager.AccountService.UpdateAsync(accountId, accountUpdatingDto));
        }

        [Fact(DisplayName = "DeleteAsync should remove account and it's user by cascade")]
        public async Task DeleteAsync_should_remove_account_and_its_user_by_cascade()
        {
            var accountsIds = (await ServiceManager.AccountService.GetAllAsync())
                .Select(a => a.Id)
                .ToArray();

            int accountIndex = _faker.Random.Int(0, accountsIds.Length - 1);

            Guid accountId = accountsIds[accountIndex];
            Guid userId = Db.Users.Where(u => u.AccountId == accountId).First().Id;

            await ServiceManager.AccountService.DeleteAsync(accountId);
            await ServiceManager.SaveChangesAsync();

            var account = await ServiceManager.AccountService.FindByIdAsync(accountId);
            var user = await ServiceManager.UserService.FindByIdAsync(userId);

            Assert.Null(account);
            Assert.Null(user);
        }

        [Fact(DisplayName = "DeleteRangeAsync should remove all accounts and all users by cascade")]
        public async Task DeleteRangeAsync_should_remove_all_accounts_and_all_users_by_cascade()
        {
            var accountsDtos = await ServiceManager.AccountService.GetAllAsync();
            var accountsIds = accountsDtos.Select(a => a.Id).ToArray();

            await ServiceManager.AccountService.DeleteRangeAsync(accountsIds);
            await ServiceManager.SaveChangesAsync();

            var newAccountsDtos = await ServiceManager.AccountService.GetAllAsync();
            var newUsersDtos = await ServiceManager.UserService.GetAllAsync();

            Assert.Empty(newAccountsDtos);
            Assert.Empty(newUsersDtos);
            Assert.Empty(Db.Accounts);
            Assert.Empty(Db.Users);
        }
    }
}
