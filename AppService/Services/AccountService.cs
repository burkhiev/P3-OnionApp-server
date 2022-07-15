using AppDomain.Entities;
using AppDomain.Exceptions.Accounts;
using AppDomain.Repositories;
using AppService.Abstractions;
using AppService.Dtos.Accounts;
using AppService.Mappers;
using AutoMapper;

namespace AppService.Services
{
    internal sealed class AccountService : IAccountService
    {
        private const int MAX_ID_GENEREATION_ATTEMPT_COUNT = 5;
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;

        public AccountService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = MapperService.Mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var accounts = _repositoryManager.AccountRepository.GetAll();
            var accountsDtos = _mapper.Map<IEnumerable<AccountDto>>(accounts);
            return accountsDtos;
        }

        public async Task<AccountDto?> FindByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, cancellationToken);
            var accountDto = account is null ? null : _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task<AccountDto> CreateAsync(AccountCreatingDto accountDto, CancellationToken cancellationToken = default)
        {
            var userData = _mapper.Map<User>(accountDto);
            var userId = Guid.NewGuid();

            for(int i = 0; i < MAX_ID_GENEREATION_ATTEMPT_COUNT; i++)
            {
                var result = await _repositoryManager.UsersRepository.FindByIdAsync(userId, cancellationToken);

                if (result is null)
                {
                    break;
                }

                userId = Guid.NewGuid();
            }

            userData.Id = userId;
            var user = await _repositoryManager.UsersRepository.AddAsync(userData, cancellationToken);

            var account = _mapper.Map<Account>(accountDto);
            var accountId = Guid.NewGuid();

            for(int i = 0; i < MAX_ID_GENEREATION_ATTEMPT_COUNT; i++)
            {
                var result = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, cancellationToken);

                if(result is null)
                {
                    break;
                }

                accountId = Guid.NewGuid();
            }

            account.Id = accountId;
            account.UserId = user.Id;

            Account newAccount = await _repositoryManager.AccountRepository.AddAsync(account, cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);

            var createdAccountDto = _mapper.Map<AccountDto>(newAccount);
            return createdAccountDto;
        }

        public async Task<AccountDto> UpdateAsync(Guid accountId, AccountUpdatingDto accountDto, CancellationToken cancellationToken = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, cancellationToken);

            if(account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            if(accountDto.Email is not null)
            {
                account.Email = accountDto.Email;
            }

            if(accountDto.Password is not null)
            {
                account.Password = accountDto.Password;
            }

            var newAccount = _repositoryManager.AccountRepository.Update(account);
            await _repositoryManager.SaveChangesAsync(cancellationToken);

            var newAccountDto = _mapper.Map<AccountDto>(newAccount);
            return newAccountDto;
        }

        public async Task<AccountDto?> DeleteAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, cancellationToken);

            if(account == null)
            {
                return null;
            }

            var deletedAccount = _repositoryManager.AccountRepository.Remove(account);
            await _repositoryManager.SaveChangesAsync(cancellationToken);

            var accountDto = _mapper.Map<AccountDto>(deletedAccount);
            return accountDto;
        }
    }
}
