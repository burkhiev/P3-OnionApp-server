// У IUserService отсутствует метод для создания User,
// т.к. User создается только при создании Account.

using AppService.Dtos.Users;

namespace AppService.Abstractions
{
    public interface IUserService
    {
        Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IQueryable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserDto> UpdateAsync(Guid userId, UserDto userDto, CancellationToken cancellationToken = default);

        Task<UserDto?> DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(Guid[] usersIds, CancellationToken cancellationToken = default);
    }
}