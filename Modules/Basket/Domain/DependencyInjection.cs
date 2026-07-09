using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Basket.Domain.BasketAggregate;

namespace Modules.Basket.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<BasketDomainService>();

        return services;
    }
}
