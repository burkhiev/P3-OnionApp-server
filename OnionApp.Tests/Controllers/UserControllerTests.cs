using AppDomain.Exceptions.Users;
using AppInfrastructure.Database;
using AppService.Dtos.Users;
using AppService.Services;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionApp.Controllers;
using OnionApp.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OnionApp.Middlewares;
using OnionApp.Tests.TestExceptions;

namespace OnionApp.Tests.Controllers
{
    public class UserControllerTests : IClassFixture<ServiceManagerFixture>, IDisposable
    {
        private readonly Faker _faker;
        private readonly ServiceManagerFixture _fixture;
        private readonly UsersController _controller;

        public UserControllerTests(ServiceManagerFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize();

            if(Db.Users.Count() == 0)
            {
                throw new InvalidFixtureDataException();
            }

            _faker = new Faker();
            _controller = new UsersController(ServiceManager);
        }

        public BusinessServiceManager ServiceManager => _fixture.ServiceManager;
        public RepositoryDbContext Db => _fixture.DbContext;

        public void Dispose()
        {
            _fixture.Reset();
        }

        [Fact(DisplayName = "GetUsersAsync returns OK result with users data")]
        public async Task GetUsersAsync_ReturnsOkResult_WithUsersData()
        {
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();

            // Act
            var response = await _controller.GetUsersAsync();

            var result = Assert.IsType<OkObjectResult>(response);
            var usersDtosFromController = Assert.IsAssignableFrom<IEnumerable<UserDto>>(result.Value);
            Assert.Equal(usersIdsFromDb.Count, usersDtosFromController.Count());
            Assert.All(usersDtosFromController, userDto => usersIdsFromDb.Contains(userDto.Id));
        }

        [Fact(DisplayName = "GetUsersAsync returns OK result with empty data")]
        public async Task GetUsersAsync_ReturnsOkResult_WithNoUsersData()
        {
            Db.Users.RemoveRange(Db.Users);
            await Db.SaveChangesAsync();
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();

            // Act
            var response = await _controller.GetUsersAsync();

            var result = Assert.IsType<OkObjectResult>(response);
            var usersDtos = Assert.IsAssignableFrom<IEnumerable<UserDto>>(result.Value);
            Assert.Empty(usersDtos);
        }

        [Fact(DisplayName = "GetUserByIdAsync returns OK result with user data when ID is valid")]
        public async Task GetUserByIdAsync_ReturnsOkResult_WithUserData_WhenIdIsValid()
        {
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();
            Guid userId = usersIdsFromDb[_faker.Random.Int(0, usersIdsFromDb.Count - 1)];

            // Act
            var response = await _controller.GetUserByIdAsync(userId);

            var result = Assert.IsType<OkObjectResult>(response);
            var userDto = Assert.IsAssignableFrom<UserDto>(result.Value);
            Assert.NotNull(userDto);
            Assert.Equal(userId, userDto.Id);
        }

        [Fact(DisplayName = "GetUserByIdAsync returns NotFound result when ID is invalid")]
        public async Task GetUserByIdAsync_ReturnsNotFoundResult_WhenIdIsInvalid()
        {
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();

            Guid userId = Guid.NewGuid();
            while(usersIdsFromDb.Contains(userId))
            {
                userId = Guid.NewGuid();
            }

            // Act
            var response = await _controller.GetUserByIdAsync(userId);

            var result = Assert.IsType<NotFoundResult>(response);
        }

        [Fact(DisplayName = "UpdateUserAsync returns ok result with updated user data when params is valid")]
        public async Task UpdateUserAsync_ReturnsOkResult_WithUpdatedUserData_WhenParamsIsValid()
        {
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();
            var userId = usersIdsFromDb[_faker.Random.Int(0, usersIdsFromDb.Count - 1)];

            string newFirstName = Guid.NewGuid().ToString();
            string newLastName = Guid.NewGuid().ToString();

            var userDto = new UserDto { Id = userId, FirstName = newFirstName, LastName = newLastName };

            // Act
            var response = await _controller.UpdateUserAsync(userDto.Id, userDto);

            var result = Assert.IsType<OkObjectResult>(response);
            var responsedUserDto = Assert.IsType<UserDto>(result.Value);

            var user = await Db.Users.FindAsync(userDto.Id);

            Assert.NotNull(user);
            Assert.Equal(userDto.Id, responsedUserDto.Id);
            Assert.Equal(user!.FirstName, responsedUserDto.FirstName);
            Assert.Equal(user.LastName, responsedUserDto.LastName);
        }

        /// <summary>
        /// <para>
        /// Проверяет, выбрасывается ли исключение, если ID передаваемое через параметр метода
        /// не соответствует ID передаваемому через аргумент типа <see cref="UserDto"/>.
        /// </para>
        /// <para>
        /// Предполагается, что данный случай будет отловлен в <see cref="CustomExceptionHandlerMiddleware"/>.
        /// </para>
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        [Fact(DisplayName = "UpdateUserAsync throws exception when user's ID param doesn't match to user's DTO ID")]
        public async Task UpdateUserAsync_ThrowsException_WhenUserIdParamDoesntMatchToUserDtosId()
        {
            Guid userId = Guid.NewGuid();
            Guid dtoUserGuid = Guid.NewGuid();

            while(userId == dtoUserGuid)
            {
                userId = Guid.NewGuid();
            }

            var userDto = new UserDto { Id = dtoUserGuid };

            // Act
            await Assert.ThrowsAsync<UserUpdateInvalidArgumentException>(async () => 
                await _controller.UpdateUserAsync(userId, userDto));
        }

        [Fact(DisplayName = "UpdateUserAsync throws error when user with specified ID was not found")]
        public async Task UpdateUserAsync_ThrowsError_WhenUserWithSpecifiedIDWasNotFound()
        {
            var usersIdsFromDb = await Db.Users.Select(u => u.Id).ToListAsync();

            Guid userId = Guid.NewGuid();
            while(usersIdsFromDb.Contains(userId))
            {
                userId = Guid.NewGuid();
            }

            var userDto = new UserDto { Id = userId };

            // Act
            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
                await _controller.UpdateUserAsync(userId, userDto));
        }
    }
}
