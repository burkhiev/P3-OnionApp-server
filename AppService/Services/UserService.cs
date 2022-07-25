using AppDomain.Entities;
using AppDomain.Exceptions.Users;
using AppDomain.Repositories;
using AppService.Abstractions;
using AppService.Dtos.Users;
using AppService.Mappers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AppService.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public UserService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = MapperService.Mapper;
            _mapperConfig = MapperService.MapperConfiguration;
        }

        public async Task<IQueryable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = _repositoryManager.UsersRepository.GetAll();
            var usersDtos = users.ProjectTo<UserDto>(_mapperConfig);
            return usersDtos;
        }

        public async Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _repositoryManager.UsersRepository.FindByIdAsync(userId, cancellationToken);
            var userDto = user is null ? null : _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> UpdateAsync(Guid userId, UserDto userDto, CancellationToken cancellationToken)
        {
            if(userId != userDto.Id)
            {
                throw new UserUpdateInvalidArgumentException(nameof(userId), userId);
            }

            var user = await _repositoryManager.UsersRepository.FindByIdAsync(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            _mapper.Map(userDto, user);

            User updatedUser = _repositoryManager.UsersRepository.Update(user);

            var result = _mapper.Map<UserDto>(updatedUser);
            return result;
        }

        public async Task<UserDto?> DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UsersRepository.FindByIdAsync(userId);

            if(user == null)
            {
                return null;
            }

            var removedUser = _repositoryManager.UsersRepository.Remove(user);
            int saveResult = await _repositoryManager.SaveChangesAsync(cancellationToken);

            if(saveResult == 0)
            {
                throw new UserNotDeletedException(user.Id);
            }


            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task DeleteRangeAsync(Guid[] usersIds, CancellationToken cancellationToken = default)
        {
            var users = _repositoryManager.UsersRepository.GetAll();
            _repositoryManager.UsersRepository.RemoveRange(users.ToArray());
        }
    }
}
