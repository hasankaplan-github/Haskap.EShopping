using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Application.Contracts;

namespace Modules.Catalog.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication(IConfiguration configuration)
        {
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IStockCheckerService, StockCheckerService>();

            return services;
        }
    }
}
