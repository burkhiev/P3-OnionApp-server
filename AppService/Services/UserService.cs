using AppDomain.Entities;
using AppDomain.Exceptions.Users;
using AppDomain.Repositories;
using AppService.Abstractions;
using AppService.Dtos.Users;
using AppService.Mappers;
using AutoMapper;

namespace AppService.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper = MapperService.Mapper;

        public UserService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = _repositoryManager.UsersRepository.GetAll();
            var usersDtos = _mapper.Map<IEnumerable<UserDto>>(users);
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
            var user = await _repositoryManager.UsersRepository.FindByIdAsync(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            _mapper.Map(userDto, user);

            User updatedUser = _repositoryManager.UsersRepository.Update(user);
            await _repositoryManager.SaveChangesAsync();

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
            await _repositoryManager.SaveChangesAsync();

            if(saveResult == 0)
            {
                throw new UserNotDeletedException(user.Id);
            }


            var result = _mapper.Map<UserDto>(user);
            return result;
        }
    }
}
