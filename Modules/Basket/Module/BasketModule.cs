using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Basket.Application;
using Modules.Basket.Application.Contracts;
using Modules.Basket.Domain;
using Modules.Basket.Infra;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.Basket.Module;

public class BasketModule : BaseModule<BasketModule>, IBasketModule
{
    public BasketModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddDomain(configuration);
            services.AddApplication(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);

            return services;
        }
    }
}
