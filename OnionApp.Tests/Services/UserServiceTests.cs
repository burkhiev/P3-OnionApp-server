using AppDomain.Exceptions.Users;
using AppInfrastructure.Database;
using AppService.Dtos.Users;
using AppService.Services;
using Bogus;
using OnionApp.Tests.Data;
using OnionApp.Tests.TestExceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnionApp.Tests.Services
{
    public class UserServiceTests : IClassFixture<ServiceManagerFixture>, IDisposable
    {
        private readonly ServiceManagerFixture _fixture;
        private readonly Faker _faker = new Faker("ru");

        public UserServiceTests(ServiceManagerFixture fixture)
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

        [Fact(DisplayName = "GetAllAsync returns all users when users count > 0")]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var usersDtos = await ServiceManager.UserService.GetAllAsync();
            var users = Db.Users.ToList();

            Assert.Equal(users.Count(), usersDtos.Count());
            Assert.All(usersDtos, userDto => Assert.Contains(users, u => u.Id == userDto.Id));
        }

        [Fact(DisplayName = "GetAllAsync returns empty collection when there are no users")]
        public async Task GetAllAsync_ReturnsEmptyUsers_WhenThereAreNoUsers()
        {
            var usersToRemove = Db.Users.ToList();
            Db.Users.RemoveRange(usersToRemove);
            Db.SaveChanges();

            var users = await ServiceManager.UserService.GetAllAsync();

            Assert.Empty(users);
        }

        [Fact(DisplayName = "FindByIdAsync returns user dto with specified ID")]
        public async Task FindByIdAsync_ReturnsUserDto_WithSpecifiedId()
        {
            var users = (await ServiceManager.UserService.GetAllAsync()).ToArray();
            int expectedUserIndex = _faker.Random.Int(0, users.Length - 1);
            var expectedUser = users[expectedUserIndex];

            var user = await ServiceManager.UserService.FindByIdAsync(expectedUser.Id);

            Assert.NotNull(user);
            Assert.Equal(expectedUser.Id, user?.Id);
        }

        [Fact(DisplayName = "FindByIdAsync returns null when there is no user with specified id")]
        public async Task FindByIdAsync_ReturnsNull_WhenThereIsNoUserWithSpecifiedId()
        {
            Guid unexistingUserId = Guid.NewGuid();
            var users = (await ServiceManager.UserService.GetAllAsync()).ToArray();

            while(users.FirstOrDefault(u => u.Id == unexistingUserId) is not null)
            {
                unexistingUserId = Guid.NewGuid();
            }

            var user = await ServiceManager.UserService.FindByIdAsync(unexistingUserId);

            Assert.Null(user);
        }

        [Fact(DisplayName = "UpdateAsync updates and returns updated user dto")]
        public async Task UpdateAsync_ReturnsUpdatedUserDto()
        {
            var usersDto = await ServiceManager.UserService.GetAllAsync();

            Assert.NotEmpty(usersDto);

            string newFirstName = Guid.NewGuid().ToString();
            string newLastName = Guid.NewGuid().ToString();
            var newBirthDate = _faker.Date.Past(2);

            UserDto userDto = usersDto.First();
            userDto.FirstName = newFirstName;
            userDto.LastName = newLastName;
            userDto.DateOfBirth = newBirthDate;

            Guid userId = userDto.Id;

            UserDto newUserDto = await ServiceManager.UserService.UpdateAsync(userDto.Id, userDto);
            await ServiceManager.SaveChangesAsync();

            Assert.NotNull(newUserDto);
            Assert.Equal(userId, newUserDto.Id);
            Assert.Equal(newFirstName, newUserDto.FirstName);
            Assert.Equal(newLastName, newUserDto.LastName);
            Assert.Equal(newBirthDate, newUserDto.DateOfBirth);
        }

        [Fact(DisplayName = "UpdateAsync throws exception if user ID is invalid")]
        public async Task UpdateAsync_ThrowsException_IfUserIdIsInvalid()
        {
            var usersIds = (await ServiceManager.UserService.GetAllAsync())
                .Select(u => u.Id)
                .ToArray();

            Guid invalidUserId = Guid.NewGuid();
            while(usersIds.Contains(invalidUserId))
            {
                invalidUserId = Guid.NewGuid();
            }

            var usersDto = new UserDto { Id = invalidUserId };

            await Assert.ThrowsAsync<UserNotFoundException>(async () => 
                await ServiceManager.UserService.UpdateAsync(invalidUserId, usersDto));
        }

        [Fact(DisplayName = "DeleteAsync removes and returns removed user id")]
        public async Task DeleteAsync_RemovesAndReturnRemovedUserId()
        {
            var usersDtos = await ServiceManager.UserService.GetAllAsync();

            Assert.NotEmpty(usersDtos);

            UserDto userDto = usersDtos.First();
            Guid userId = userDto.Id;

            var deleteUser = await ServiceManager.UserService.DeleteAsync(userId);
            await ServiceManager.SaveChangesAsync();

            Assert.NotNull(deleteUser);
            Assert.Equal(userId, deleteUser!.Id);
        }
    }
}
