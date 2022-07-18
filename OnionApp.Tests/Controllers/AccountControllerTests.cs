using AppInfrastructure.Database;
using AppService.Dtos.Accounts;
using AppService.Services;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionApp.Controllers;
using OnionApp.Tests.Fixtures;
using OnionApp.Tests.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppDomain.Exceptions.Accounts;

namespace OnionApp.Tests.Controllers
{
    public class AccountControllerTests : IClassFixture<ServiceManagerFixture>, IDisposable
    {
        private readonly Faker _faker = new Faker("ru");
        private readonly ServiceManagerFixture _fixture;
        private readonly AccountsController _controller;

        public AccountControllerTests(ServiceManagerFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize();

            if(Db.Accounts.Count() == 0)
            {
                throw new InvalidFixtureDataException();
            }

            _controller = new AccountsController(ServiceManager);
        }

        public RepositoryDbContext Db => _fixture.DbContext;
        public BusinessServiceManager ServiceManager => _fixture.ServiceManager;

        public void Dispose()
        {
            _fixture.Reset();
        }

        [Fact(DisplayName = "GetAccountsAsync returns OkResult with users' data when there are some accounts in the DB")]
        public async Task GetAccountsAsync_ReturnsOkResult_WithUsersData_WhenThereAreSomeAccountsInTheDB()
        {
            var response = await _controller.GetAccountsAsync();

            var result = Assert.IsType<OkObjectResult>(response);
            var accountsDtos = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(result.Value);
            Assert.NotEmpty(accountsDtos);
            Assert.Equal(Db.Accounts.Count(), accountsDtos.Count());
        }

        [Fact(DisplayName = "GetAccountsAsync returns OkResult with empty users' data when there are not any accounts in the DB")]
        public async Task GetAccountsAsync_ReturnsOkResult_WithEmptyUsersData_WhenThereAreNotAnyAccountsInTheDB()
        {
            Db.Accounts.RemoveRange(Db.Accounts);
            Db.SaveChanges();

            var response = await _controller.GetAccountsAsync();

            var result = Assert.IsType<OkObjectResult>(response);
            var accountsDtos = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(result.Value);
            Assert.Empty(accountsDtos);
        }

        [Fact(DisplayName = "GetAccountByIdAsync returns OkResult with account's data when user with specified ID exists")]
        public async Task GetAccountByIdAsync_ReturnsOkResult_WithAccountData_WhenUserWithSpecifiedIdExists()
        {
            var accountsIds = await Db.Accounts.Select(a => a.Id).ToListAsync();
            Guid accountId = accountsIds[_faker.Random.Int(0, accountsIds.Count - 1)];

            var response = await _controller.GetAccountByIdAsync(accountId);

            var result = Assert.IsType<OkObjectResult>(response);
            var accountDto = Assert.IsAssignableFrom<AccountDto>(result.Value);

            Assert.NotNull(accountDto);
            Assert.Equal(accountId, accountDto.Id);
        }

        [Fact(DisplayName = "GetAccountByIdAsync returns NotFoundResult when user with specified ID doesn't ID exists")]
        public async Task GetAccountByIdAsync_ReturnsNotFoundResult_WhenUserWithSpecifiedIdDoesntExist()
        {
            var accountsIds = await Db.Accounts.Select(a => a.Id).ToListAsync();

            Guid accountId = Guid.NewGuid();
            while(accountsIds.Contains(accountId))
            {
                accountId = Guid.NewGuid();
            }

            var response = await _controller.GetAccountByIdAsync(accountId);

            var result = Assert.IsType<NotFoundResult>(response);
        }

        [Fact(DisplayName = "CreateAccountAsync creates account and returns CreatedAtActionResult with account's data when args is valid")]
        public async Task CreateAccountAsync_CreatesAccount_And_ReturnsCreatedAtActionResult_WithAccountsData_WhenArgsIsValid()
        {
            string firstName = _faker.Name.FirstName();
            string lastName = _faker.Name.LastName();
            DateTime dateOfBirth = _faker.Date.Past();

            var accountDto = new AccountCreatingDto
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password(),
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth
            };

            var response = await _controller.CreateAccountAsync(accountDto);

            var result = Assert.IsType<CreatedAtActionResult>(response);
            var createdAccountDto = Assert.IsAssignableFrom<AccountDto>(result.Value);

            Assert.Equal(nameof(_controller.GetAccountByIdAsync), result.ActionName);

            Assert.Single(result.RouteValues);
            Guid routeAccountId = Assert.IsType<Guid>(result.RouteValues.First().Value);

            var accountFromDb = Db.Accounts.Find(routeAccountId);
            Assert.NotNull(accountFromDb);
            Assert.Equal(accountFromDb.Id, createdAccountDto.Id);
            Assert.Equal(accountFromDb.Id, routeAccountId);
            Assert.Equal(accountFromDb.Email, accountDto.Email);
            Assert.Equal(accountFromDb.Email, createdAccountDto.Email);
            Assert.Equal(accountFromDb.Password, accountDto.Password);

            var userFromDb = await Db.Users.FirstOrDefaultAsync(u => u.AccountId == accountFromDb.Id);
            Assert.NotNull(userFromDb);
            Assert.Equal(userFromDb.FirstName, firstName);
            Assert.Equal(userFromDb.LastName, lastName);
            Assert.Equal(userFromDb.DateOfBirth, dateOfBirth);
        }

        [Fact(DisplayName = "UpdateAccountAsync updates account data and returns OkResult with updated user's data")]
        public async Task UpdateAccountAsync_UpdatesAccountData_And_ReturnsOkResult_WithUpdatedUsersData()
        {
            var accountsIds = await Db.Accounts.Select(a => a.Id).ToListAsync();
            Guid accountId = accountsIds[_faker.Random.Int(0, accountsIds.Count - 1)];

            string newEmail = "WTF it's going on!!!";
            string newPassword = "my super secret password";

            var accountUpdatingDto = new AccountUpdatingDto
            {
                AccountId = accountId,
                Email = newEmail,
                Password = newPassword
            };

            var response = await _controller.UpdateAccountAsync(accountId, accountUpdatingDto);

            var result = Assert.IsType<OkObjectResult>(response);
            var accountDto = Assert.IsType<AccountDto>(result.Value);
            var accountFromDb = await Db.Accounts.FindAsync(new object[] { accountId });

            if(accountFromDb is null)
            {
                string methodName = nameof(UpdateAccountAsync_UpdatesAccountData_And_ReturnsOkResult_WithUpdatedUsersData);
                throw new InvalidTestException(GetType(), methodName);
            }

            Assert.Equal(accountId, accountDto.Id);
            Assert.Equal(newEmail, accountDto.Email);
            Assert.Equal(newEmail, accountFromDb.Email);
            Assert.Equal(newPassword, accountFromDb.Password);
        }

        [Fact(DisplayName = "UpdateAccountAsync throws exception when argument account ID is not match to account dto ID")]
        public async Task UpdateAccountAsync_ThrowsException_WhenArgumentAccountIdIsNotMatchToAccountDtoId()
        {
            var accountDto = new AccountUpdatingDto { AccountId = Guid.NewGuid() };
            Guid accountId = Guid.NewGuid();

            while(accountId == accountDto.AccountId)
            {
                accountId = Guid.NewGuid();
            }

            await Assert.ThrowsAsync<AccountInvalidArgumentForUpdateException>(async () =>
                await _controller.UpdateAccountAsync(accountId, accountDto));
        }

        [Fact(DisplayName = "UpdateAccountAsync throws exception when account with specified ID doesn't exist")]
        public async Task UpdateAccountAsync_ThrowsException_WhenAccountWithSpecifiedIdDoesntExist()
        {
            var accountsIds = await Db.Accounts.Select(a => a.Id).ToListAsync();
            Guid accountId = Guid.NewGuid();

            while(accountsIds.Contains(accountId))
            {
                accountId = Guid.NewGuid();
            }

            var accountDto = new AccountUpdatingDto { AccountId = accountId };

            await Assert.ThrowsAsync<AccountNotFoundException>(async () =>
                await _controller.UpdateAccountAsync(accountId, accountDto));
        }

        [Fact(DisplayName = "DeleteAccountByIdAsync returns NoContentResult and removes account with specified ID")]
        public async Task DeleteAccountByIdAsync_ReturnsNoContentResult_And_RemovesAccount_WithSpecifiedId()
        {
            var accountsIds = await Db.Accounts.Select(a => a.Id).ToListAsync();
            Guid accountId = accountsIds[_faker.Random.Int(0, accountsIds.Count - 1)];

            var response = await _controller.DeleteAccountByIdAsync(accountId);

            var result = Assert.IsType<NoContentResult>(response);
            var accountFromDb = await Db.Accounts.FindAsync(accountId);
            Assert.Null(accountFromDb);
        }
    }
}
