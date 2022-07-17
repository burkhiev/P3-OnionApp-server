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
using Microsoft.AspNetCore.Mvc;
using OnionApp.Filters;
using OnionApp.Utilities.ResponseTypes;
using System.Net.Mime;

namespace OnionApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)]
    public class UsersController : Controller
    {
        private readonly IBusinessServiceManager _serviceManager;

        public UsersController(IBusinessServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>All users' data.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var qUsersDtos = await _serviceManager.UserService.GetAllAsync();
            var usersDtos = qUsersDtos.ToList();

            return Ok(usersDtos);
        }

        /// <summary>
        /// Returns user with specified ID.
        /// </summary>
        /// <param name="userId">User ID (Guid).</param>
        /// <returns>Specified ID user's data.</returns>
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound, MediaTypeNames.Text.Plain)]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId)
        {
            var userDto = await _serviceManager.UserService.FindByIdAsync(userId);

            if(userDto is null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        /// <summary>
        /// Updates user.
        /// </summary>
        /// <param name="userId">User ID (Guid).</param>
        /// <param name="userDto">Object which contain user's data for update.</param>
        /// <returns>Updated user's data.</returns>
        /// <response code="200">qwerty</response>
        /// <response code="400">WTF</response>
        [HttpPut("{userId:guid}")]
        [TypeFilter(typeof(UserDtosValidationFilterAttribute))]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateUserAsync(Guid userId, [FromBody] UserDto userDto)
        {
            var resultUserDto = await _serviceManager.UserService.UpdateAsync(userId, userDto);
            await _serviceManager.SaveChangesAsync();

            return Ok(resultUserDto);
        }
    }
}
