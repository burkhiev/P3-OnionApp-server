using AppService.Abstractions;
using AppService.Dtos.Accounts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace OnionApp.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly IBusinessServiceManager _serviceManager;
        private readonly IMapper _mapper;

        public AccountsController(IBusinessServiceManager serviceManager, IMapper mapper)
        {
            _serviceManager = serviceManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(CancellationToken ct = default)
        {
            var users = await _serviceManager.AccountService.GetAllAsync(ct);

            return Ok(users);
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountById(Guid accountId, CancellationToken ct = default)
        {
            var user =  await _serviceManager.AccountService.FindByIdAsync(accountId, ct);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreatingDto accountDto, CancellationToken ct = default)
        {
            AccountDto createdAccountDto = await _serviceManager.AccountService.CreateAsync(accountDto, ct);

            return CreatedAtAction(nameof(GetAccountById), new { accountId = createdAccountDto.Id }, createdAccountDto);
        }

        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> UpdateAccount(Guid accountId, [FromBody] AccountUpdatingDto accountDto, CancellationToken ct = default)
        {
            await _serviceManager.AccountService.UpdateAsync(accountId, accountDto, ct);

            return NoContent();
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> DeleteAccountById(Guid accountId, CancellationToken ct = default)
        {
            await _serviceManager.AccountService.DeleteAsync(accountId, ct);

            return NoContent();
        }
    }
}
