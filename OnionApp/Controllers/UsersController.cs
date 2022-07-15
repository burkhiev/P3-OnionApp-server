using AppService.Abstractions;
using AppService.Dtos.Users;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace OnionApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IBusinessServiceManager _serviceManager;
        private readonly IValidator<UserDto> _userDtoValidator;

        public UsersController(IBusinessServiceManager serviceManager, IValidator<UserDto> userDtoValidator)
        {
            _serviceManager = serviceManager;
            _userDtoValidator = userDtoValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken = default)
        {
            var usersDtos = await _serviceManager.UserService.GetAllAsync(cancellationToken);

            return Ok(usersDtos);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken = default)
        {
            var userDto = await _serviceManager.UserService.FindByIdAsync(userId, cancellationToken);

            return Ok(userDto);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserDto userDto, CancellationToken cancellationToken = default)
        {
            await _serviceManager.UserService.UpdateAsync(userId, userDto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserById(Guid userId, CancellationToken cancellationToken = default)
        {
            await _serviceManager.UserService.DeleteAsync(userId, cancellationToken);

            return NoContent();
        }
    }
}
