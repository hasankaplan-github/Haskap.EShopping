using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Modules.Account.Domain;
using Modules.Account.Domain.RoleAggregate.Events;

namespace Modules.Account.Application.Role;
public class RolePermissionsCacheContentUpdatedEventHandler
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly IAccountDbContext _accountDbContext;

    public RolePermissionsCacheContentUpdatedEventHandler(
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        IAccountDbContext accountDbContext)
    {
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _accountDbContext = accountDbContext;
    }

    public async Task Handle(RolePermissionsCacheContentUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _memoryCache.Remove(_baseCacheKeyProvider.GetRolePermissionsCacheKey(notification.RoleId));

        var accountIds = await _accountDbContext.AccountRole
            .Where(x => x.RoleId == notification.RoleId)
            .Select(x => x.AccountId)
            .ToListAsync(cancellationToken);

        if (accountIds is null)
        {
            return;
        }

        foreach (var accountId in accountIds) 
        {
            _memoryCache.Remove(_baseCacheKeyProvider.GetAllPermissionsCacheKey(accountId));
        }
    }
}
