using Haskap.DddBase.Infra.Providers;
using Haskap.EShopping.Domain.Providers;

namespace Haskap.EShopping.Infra.Providers;

public class CacheKeyProvider : BaseCacheKeyProvider, ICacheKeyProvider
{
    public string GetOpenLoginCacheKey(Guid userId, Guid loginId)
    {
        return $"OpenLogin_{userId}_{loginId}_CacheKey";
    }
}
