using Haskap.DddBase.Domain.Providers;

namespace Haskap.EShopping.Domain.Providers;

public interface ICacheKeyProvider : IBaseCacheKeyProvider
{
    string GetOpenLoginCacheKey(Guid userId, Guid loginId);
}
