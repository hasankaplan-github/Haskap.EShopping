using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts;
using Modules.Order.Application;
using Modules.Order.Domain.Shared;
using Modules.Order.Infra;

namespace Modules.Order.Module;

public class OrderModule : BaseModule<OrderModule>, IOrderModule
{
    public OrderModule(
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
