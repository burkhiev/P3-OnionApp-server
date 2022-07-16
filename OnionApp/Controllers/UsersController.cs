// ***** Notes *****
//
//   Возможность удаления сущностей User в контроллере отсутствует.
// Удаление предусмотрено только, при удалении связанной с User сущности Account.
//
//   Сама логика разделения данных выглядела весьма привлекающей перед написанием
// данного учебного решения, но на практике выяснилось, что будет лучше,
// если в будущем подобные сущности я буду объединять в одну.
//

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
            var qUsersDtos = await _serviceManager.UserService.GetAllAsync(cancellationToken);
            var usersDtos = qUsersDtos.ToList();

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
            var validationResult = _userDtoValidator.Validate(userDto);

            if (validationResult.IsValid)
            {
                await _serviceManager.UserService.UpdateAsync(userId, userDto, cancellationToken);
                return NoContent();
            }

            foreach(var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return BadRequest(ModelState);
        }
    }
}
