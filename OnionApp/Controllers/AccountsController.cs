using AppService.Abstractions;
using AppService.Dtos.Accounts;
using Microsoft.AspNetCore.Mvc;
using OnionApp.Filters;
using OnionApp.Utilities.ResponseTypes;
using System.Net.Mime;

namespace OnionApp.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ClientErrorResponse), StatusCodes.Status500InternalServerError, MediaTypeNames.Application.Json)]
    public class AccountsController : Controller
    {
        private readonly IBusinessServiceManager _serviceManager;

        public AccountsController(IBusinessServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var users = await _serviceManager.AccountService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{accountId:guid}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountByIdAsync(Guid accountId)
        {
            var user =  await _serviceManager.AccountService.FindByIdAsync(accountId);

            if(user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [TypeFilter(typeof(AccountCreatingDtosFilterAttrubute))]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreatingDto accountDto)
        {
            AccountDto createdAccountDto = await _serviceManager.AccountService.CreateAsync(accountDto);
            await _serviceManager.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccountByIdAsync), new { accountId = createdAccountDto.Id }, createdAccountDto);
        }

        [HttpPut("{accountId:guid}")]
        [TypeFilter(typeof(AccountUpdatingDtosFilterAttribute))]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateAccountAsync(Guid accountId, [FromBody] AccountUpdatingDto accountDto)
        {
            var updatedAccountDto = await _serviceManager.AccountService.UpdateAsync(accountId, accountDto);
            await _serviceManager.SaveChangesAsync();

            return Ok(updatedAccountDto);
        }

        [HttpDelete("{accountId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAccountByIdAsync(Guid accountId)
        {
            await _serviceManager.AccountService.DeleteAsync(accountId);
            await _serviceManager.SaveChangesAsync();

            return NoContent();
        }
    }
}
