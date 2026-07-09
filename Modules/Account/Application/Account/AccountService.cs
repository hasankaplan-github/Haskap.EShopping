using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Application.Dtos.Role;
using Modules.Account.Domain;
using Modules.Account.Domain.AccountAggregate;
using Modules.Account.Domain.AccountAggregate.Events;
using Modules.Account.Domain.AccountAggregate.Exceptions;
using Modules.Account.Domain.ExternalServices;

namespace Modules.Account.Application.Account;
public class AccountService : UseCaseService, IAccountService
{
    private readonly IAccountDbContext _accountDbContext;
    private readonly IGoogleReCaptchaService _googleReCaptchaService;
    private readonly IHashProvider _hashProvider;
    private readonly ICacheKeyProvider _cacheKeyProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IEventPublisher _eventPublisher;

    public AccountService(
        IAccountDbContext accountDbContext,
        IGoogleReCaptchaService googleReCaptchaService,
        IHashProvider hashProvider,
        IMemoryCache memoryCache,
        ICurrentUserIdProvider currentUserIdProvider,
        ICacheKeyProvider cacheKeyProvider,
        IEventPublisher eventPublisher)
    {
        _accountDbContext = accountDbContext;
        _googleReCaptchaService = googleReCaptchaService;
        _hashProvider = hashProvider;
        _memoryCache = memoryCache;
        _currentUserIdProvider = currentUserIdProvider;
        _cacheKeyProvider = cacheKeyProvider;
        _eventPublisher = eventPublisher;
    }

    public async Task<List<AccountOutputDto>> GetUsersByIdAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        var accounts = await _accountDbContext.Account
            .Where(x => userIds.Contains(x.Id))
            .Select(x => new AccountOutputDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.Credentials.Username,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                IsLocked = x.IsLocked,
                FailedAttemptCount = x.LoginAttempt.FailedAttemptCount,
                LastFailedAttemptUtcDateTime = x.LoginAttempt.LastFailedAttemptUtcDateTime
            })
            .ToListAsync(cancellationToken);

        return accounts;
    }

    public async Task<JqueryDataTableResult> SearchAsync(Dtos.Account.SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = (from user in _accountDbContext.Account
                     select new AccountOutputDto
                     {
                         Id = user.Id,
                         FirstName = user.FirstName,
                         LastName = user.LastName,
                         Username = user.Credentials.Username,
                     });

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.FirstName is not null)
        {
            filtered = true;
            query = query.Where(x => x.FirstName.Contains(inputDto.FirstName));
        }

        if (inputDto.LastName is not null)
        {
            filtered = true;
            query = query.Where(x => x.LastName.Contains(inputDto.LastName));
        }

        if (inputDto.Username is not null)
        {
            filtered = true;
            query = query.Where(x => x.Username.Contains(inputDto.Username));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.FirstName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.FirstName);
                }
            }
            else if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.LastName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.LastName);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Username);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Username);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.FirstName);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var accountOutputDtos = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = accountOutputDtos
        };
    }

    public async Task ResetFailedLoginAttemptsAndUnlockAsync(ResetFailedLoginAttemptsAndUnlockInputDto inputDto, CancellationToken cancellationToken)
    {
        var account = await _accountDbContext.Account
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        account.LoginAttempt.Reset();
        account.Unlock();

        await _accountDbContext.SaveChangesAsync();
    }

    public async Task UpdateRolesAsync(Dtos.Account.UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _accountDbContext.Account
            .Include(x => x.Roles)
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        user.UpdateRoles(inputDto.UncheckedRoles, inputDto.CheckedRoles);

        await _accountDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new AccountPermissionsCacheContentUpdatedDomainEvent(user.Id), cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetRolesAsync(GetRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var roles = (from account in _accountDbContext.Account
                     join accountRole in _accountDbContext.AccountRole on account.Id equals accountRole.AccountId
                     join role in _accountDbContext.Role on accountRole.RoleId equals role.Id
                     where account.Id == inputDto.UserId
                     select new RoleOutputDto
                     {
                         Id = role.Id,
                         Name = role.Name
                     })
                    .ToList();

        return roles;
    }

    public async Task UpdatePermissionsAsync(Dtos.Account.UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _accountDbContext.Account
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        user.UpdatePermissions(inputDto.UncheckedPermissions, inputDto.CheckedPermissions);

        await _accountDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new AccountPermissionsCacheContentUpdatedDomainEvent(user.Id), cancellationToken);
    }


    public async Task<HashSet<string>> GetUserPermissionsAsync(GetUserPermissionsInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(_cacheKeyProvider.GetUserPermissionsCacheKey(inputDto.UserId), async cacheEntry =>
        {
            var userId = _currentUserIdProvider.CurrentUserId;
            var userCts = _memoryCache.Get<CancellationTokenSource>(_cacheKeyProvider.GetUserCancellationTokenSourceCacheKey(userId!.Value));
            cacheEntry.AddExpirationToken(new CancellationChangeToken(userCts.Token));
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);

            var user = await _accountDbContext.Account
                                            .Where(x => x.Id == inputDto.UserId)
                                            .FirstAsync(cancellationToken);

            var userPermissions = user.Permissions.Select(x => x.Name).ToHashSet();

            return userPermissions;
        });

        return cachedValue;
    }

    public async Task UpdateAsync(Dtos.Account.UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _accountDbContext.Account
            .Where(x => x.Id == _currentUserIdProvider.CurrentUserId.Value)
            .FirstAsync(cancellationToken);

        user.Update(
            inputDto.FirstName,
            inputDto.LastName,
            user.Credentials.Username,
            inputDto.CurrentPassword,
            _hashProvider,
            _accountDbContext.Account);

        await _accountDbContext.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(ChangePasswordInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var user = await _accountDbContext.Account
            .FindAsync(new object[] { _currentUserIdProvider.CurrentUserId!.Value }, cancellationToken);

        user!.Credentials.Password.Change(
            inputDto.CurrentPassword,
            inputDto.NewPassword,
            inputDto.NewPasswordConfirmation,
            user.Credentials.Username,
            _hashProvider);

        await _accountDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SignOutAsync(CancellationToken cancellationToken)
    {
        var userCts = _memoryCache.Get<CancellationTokenSource>(_cacheKeyProvider.GetUserCancellationTokenSourceCacheKey(_currentUserIdProvider.CurrentUserId!.Value));
        userCts?.Cancel();

        await SignOutOfOpenLoginAsync(_currentUserIdProvider.CurrentUserId!.Value, _currentUserIdProvider.CurrentLoginId!.Value, cancellationToken);
    }

    public async Task<AccountOutputDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var output = await _accountDbContext.Account
                        .Where(x => x.Id == userId)
                        .Select(x => new AccountOutputDto
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Username = x.Credentials.Username,
                            EmailAddress = x.EmailAddress,
                            PhoneNumber = x.PhoneNumber,
                            IsLocked = x.IsLocked,
                            FailedAttemptCount = x.LoginAttempt.FailedAttemptCount,
                            LastFailedAttemptUtcDateTime = x.LoginAttempt.LastFailedAttemptUtcDateTime
                        })
                        .FirstOrDefaultAsync(cancellationToken);

        return output;
    }

    public async Task<IReadOnlyList<OpenLoginOutputDto>> GetOpenLoginsOfUserAsync(Guid? tenantId, Guid userId, CancellationToken cancellationToken)
    {
        var openLogins = await _accountDbContext.Account
            .Include(x => x.OpenLogins)
            .Where(x => x.Id == userId)
            .SelectMany(x => x.OpenLogins.Where(y => y.Id != _currentUserIdProvider.CurrentLoginId))
            .ToListAsync(cancellationToken);

        var output = openLogins.Select(x => x.ToOpenLoginOutputDto()).ToList().AsReadOnly();

        return output;
    }


    public async Task SignOutOfOpenLoginAsync(Guid accountId, Guid openLoginId, CancellationToken cancellationToken)
    {
        var account = await _accountDbContext.Account
            .Include(x => x.OpenLogins.Where(y => y.Id == openLoginId))
            .Where(x => x.Id == accountId)
            .FirstAsync(cancellationToken);

        account.SignOut(openLoginId);

        await _accountDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CreateOutputDto> CreateAsync(CreateInputDto input, CancellationToken cancellationToken)
    {
        var password = Password.Generate(_hashProvider);

        var credentials = new Credentials(
            input.Username,
            password);

        var account = new Domain.AccountAggregate.Account(
            GuidGenerator.CreateSimpleGuid(),
            input.FirstName,
            input.LastName,
            input.EmailAddress,
            input.PhoneNumber,
            credentials,
            _accountDbContext.Account);

        _accountDbContext.Account.Add(account);

        await _accountDbContext.SaveChangesAsync(cancellationToken);

        return new() { ClearPassword = password.ClearValue };
    }

    public async Task SetLoginLastSeenDateTimeAsync(Guid accountId, Guid loginId, DateTime utcCurrentLastSeen, CancellationToken cancellationToken)
    {
        var lastSeenDiff = DateTime.UtcNow - utcCurrentLastSeen;
        if (lastSeenDiff < TimeSpan.FromMinutes(1))
        {
            return;
        }

        var account = await _accountDbContext.Account
            .Include(x => x.OpenLogins.Where(y => y.Id == loginId))
            .Where(x => x.Id == accountId)
            .FirstAsync(cancellationToken);

        var openLogin = account.OpenLogins.FirstOrDefault();

        if (openLogin is null)
        {
            return;
        }

        openLogin.SetLastSeenDateTime();

        await _accountDbContext.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove(_cacheKeyProvider.GetOpenLoginCacheKey(accountId, loginId));
    }

    public async Task<OpenLoginOutputDto?> GetOpenLoginAsync(Guid accountId, Guid loginId, CancellationToken cancellationToken)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(_cacheKeyProvider.GetOpenLoginCacheKey(accountId, loginId), async cacheEntry =>
        {
            //var userId = _currentUserIdProvider.CurrentUserId;
            var userCts = _memoryCache.Get<CancellationTokenSource>(_cacheKeyProvider.GetUserCancellationTokenSourceCacheKey(accountId));
            cacheEntry.AddExpirationToken(new CancellationChangeToken(userCts.Token));

            var openLogin = await _accountDbContext.OpenLogin
                .Where(x => x.AccountId == accountId && x.Id == loginId)
                .FirstOrDefaultAsync(cancellationToken);

            return openLogin;
        });

        return cachedValue?.ToOpenLoginOutputDto();
    }

    public async Task<HashSet<string>> GetAllPermissionsAsync(GetAllPermissionsInputDto input, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _memoryCache.GetOrCreateAsync(_cacheKeyProvider.GetAllPermissionsCacheKey(input.AccountId), async cacheEntry =>
        {
            //var userCts = _memoryCache.Get<CancellationTokenSource>(_cacheKeyProvider.GetUserCancellationTokenSourceCacheKey(_currentUserIdProvider.CurrentUserId ?? inputDto.UserId));
            //cacheEntry.AddExpirationToken(new CancellationChangeToken(userCts.Token));
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);

            var user = await _accountDbContext.Account
                                            .Include(x => x.Roles)
                                            .Where(x => x.Id == input.AccountId)
                                            .FirstAsync(cancellationToken);

            var allPermissions = user.GetAllPermissions(_accountDbContext.Role);

            return allPermissions;
        });

        return cachedValue;
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto inputDto, CancellationToken cancellationToken = default)
    {
        //await _googleReCaptchaService.VerifyLoginAsync(inputDto.ReCaptchaToken, cancellationToken);

        var account = await _accountDbContext.Account
            .Where(x => x.Credentials.Username == inputDto.Username)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            throw new WrongUsernameOrPasswordException();
        }

        if (account.LoginAttempt.ShouldResetFailedAttempts())
        {
            account.LoginAttempt.Reset();
            account.Unlock();
            await _accountDbContext.SaveChangesAsync(cancellationToken);
        }

        if (account.IsLocked)
        {
            throw new AccountIsLockedException();
        }

        if (!_hashProvider.IsVerified(account.Credentials.Password.HashedValue, inputDto.Password, account.Credentials.Password.Salt.Value))
        {
            account.LoginAttempt.SetFailedAttempt();
            if (account.LoginAttempt.ShouldLockAccount())
            {
                account.Lock();
            }
            await _accountDbContext.SaveChangesAsync(cancellationToken);

            throw new WrongUsernameOrPasswordException();
        }

        var loginId = account.CreateNewLogin(inputDto.RemoteIpAddress, inputDto.UserAgentString);
        await _accountDbContext.SaveChangesAsync(cancellationToken);

        var cacheKey = _cacheKeyProvider.GetUserCancellationTokenSourceCacheKey(account.Id);
        var existingUserCts = _memoryCache.Get<CancellationTokenSource>(cacheKey);
        existingUserCts?.Cancel();

        var userCts = new CancellationTokenSource();
        _memoryCache.Set(cacheKey, userCts, new CancellationChangeToken(userCts.Token));


        var output = new LoginOutputDto
        {
            AccountId = account.Id,
            UserFirstName = account.FirstName,
            UserLastName = account.LastName,
            LoginId = loginId
        };

        return output;
    }

}
