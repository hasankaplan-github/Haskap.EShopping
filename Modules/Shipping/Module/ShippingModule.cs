using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.Shipping.Application;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Infra;

namespace Modules.Shipping.Module;

public class ShippingModule : BaseModule<ShippingModule>, IShippingModule
{
    public ShippingModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            //services.AddDomain(configuration);
            services.AddApplication(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);

            return services;
        }
    }
}
