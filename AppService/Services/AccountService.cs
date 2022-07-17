using AppDomain.Entities;
using AppDomain.Exceptions.Accounts;
using AppDomain.Repositories;
using AppService.Abstractions;
using AppService.Dtos.Accounts;
using AppService.Mappers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;

namespace AppService.Services
{
    internal sealed class AccountService : IAccountService
    {
        private const int MAX_ID_GENEREATION_ATTEMPT_COUNT = 5;
        private IRepositoryManager _repositoryManager;
        private IMapper _mapper;
        private MapperConfiguration _mapperConfig;

        public AccountService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = MapperService.Mapper;
            _mapperConfig = MapperService.MapperConfiguration;
        }

        public async Task<IQueryable<AccountDto>> GetAllAsync(CancellationToken ct = default)
        {
            var accounts = _repositoryManager.AccountRepository.GetAll();
            var accountsDtos = accounts.ProjectTo<AccountDto>(_mapperConfig);
            return accountsDtos;
        }

        public async Task<AccountDto?> FindByIdAsync(Guid accountId, CancellationToken ct = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, ct);
            var accountDto = account is null ? null : _mapper.Map<AccountDto>(account);
            return accountDto;
        }

        public async Task<AccountDto> CreateAsync(AccountCreatingDto accountDto, CancellationToken ct = default)
        {
            var accountData = _mapper.Map<Account>(accountDto);
            var accountId = Guid.NewGuid();

            for(int i = 0; i < MAX_ID_GENEREATION_ATTEMPT_COUNT; i++)
            {
                var result = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, ct);

                if(result is null)
                {
                    break;
                }

                accountId = Guid.NewGuid();
            }

            accountData.Id = accountId;
            var account = await _repositoryManager.AccountRepository.AddAsync(accountData, ct);


            var userData = _mapper.Map<User>(accountDto);
            var userId = Guid.NewGuid();

            for(int i = 0; i < MAX_ID_GENEREATION_ATTEMPT_COUNT; i++)
            {
                var result = await _repositoryManager.UsersRepository.FindByIdAsync(userId, ct);

                if (result is null)
                {
                    break;
                }

                userId = Guid.NewGuid();
            }

            userData.Id = userId;
            userData.AccountId = account.Id;
            var user = await _repositoryManager.UsersRepository.AddAsync(userData, ct);


            var createdAccountDto = _mapper.Map<AccountDto>(account);
            return createdAccountDto;
        }

        public async Task<AccountDto> UpdateAsync(Guid accountId, AccountUpdatingDto accountDto, CancellationToken ct = default)
        {
            if(accountId == Guid.Empty || accountDto.AccountId != accountId)
            {
                throw new AccountInvalidArgumentForUpdateException(accountId);
            }

            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, ct);

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

            var newAccountDto = _mapper.Map<AccountDto>(newAccount);
            return newAccountDto;
        }

        public async Task<AccountDto?> DeleteAsync(Guid accountId, CancellationToken ct = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId, ct);

            if(account == null)
            {
                return null;
            }

            var deletedAccount = _repositoryManager.AccountRepository.Remove(account);

            var accountDto = _mapper.Map<AccountDto>(deletedAccount);
            return accountDto;
        }

        public async Task DeleteRangeAsync(Guid[] accountsIds, CancellationToken ct = default)
        {
            var accounts = _repositoryManager.AccountRepository.GetAll();
            _repositoryManager.AccountRepository.RemoveRange(accounts.ToArray());
        }

        public async Task<AccountFullDtoWithoutIncludes> FindByIdFullWithoutIncludes(Guid accountId, CancellationToken ct = default)
        {
            var account = await _repositoryManager.AccountRepository.FindByIdAsync(accountId);

            if(account is null)
            {
                throw new AccountNotFoundException(accountId);
            }

            var accountWithDetailsDto = _mapper.Map<AccountFullDtoWithoutIncludes>(account);
            return accountWithDetailsDto;
        }
    }
}
