using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Shipping.Application.Contracts;

namespace Modules.Shipping.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IShippingService, ShippingService>();

        return services;
    }
}
