using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.Caching.Memory;
using Modules.Account.Domain.AccountAggregate.Events;

namespace Modules.Account.Application.Account;
public class AccountPermissionsCacheContentUpdatedEventHandler : IEventHandler<AccountPermissionsCacheContentUpdatedDomainEvent>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;

    public AccountPermissionsCacheContentUpdatedEventHandler(
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider)
    {
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
    }

    public async Task HandleAsync(AccountPermissionsCacheContentUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        _memoryCache.Remove(_baseCacheKeyProvider.GetUserPermissionsCacheKey(@event.AccountId));
        _memoryCache.Remove(_baseCacheKeyProvider.GetAllPermissionsCacheKey(@event.AccountId));
    }
}
