using AppDomain.Entities;
using AppService.Dtos.Accounts;
using System.Linq.Expressions;

namespace AppService.Abstractions
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAsync(AccountCreatingDto dto, CancellationToken ct = default);
        Task<AccountDto?> DeleteAsync(Guid accountId, CancellationToken ct = default);
        Task<AccountDto?> FindByIdAsync(Guid accountId, CancellationToken ct = default);
        Task<AccountFullDtoWithoutIncludes> FindByIdFullWithoutIncludes(Guid accountId, CancellationToken ct = default);
        Task<IQueryable<AccountDto>> GetAllAsync(CancellationToken ct = default);
        Task<AccountDto> UpdateAsync(Guid accountId, AccountUpdatingDto accountDto, CancellationToken ct = default);
        Task DeleteRangeAsync(Guid[] accountsIds, CancellationToken ct = default);
    }
}