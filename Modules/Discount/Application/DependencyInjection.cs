using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Discount.Application.Contracts;

namespace Modules.Discount.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDiscountService, DiscountService>();

        return services;
    }
}
