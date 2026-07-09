using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Basket.Application.Contracts;

namespace Modules.Basket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IBasketService, BasketService>();

        return services;
    }
}
