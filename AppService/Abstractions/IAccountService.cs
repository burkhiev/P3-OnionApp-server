using AppDomain.Entities;
using AppService.Dtos.Accounts;
using AppService.Dtos.Users;

namespace AppService.Abstractions
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAsync(AccountCreatingDto dto, CancellationToken cancellationToken = default);
        Task<AccountDto?> DeleteAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<AccountDto?> FindByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AccountDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AccountDto> UpdateAsync(Guid accountId, AccountUpdatingDto accountDto, CancellationToken cancellationToken = default);
    }
}