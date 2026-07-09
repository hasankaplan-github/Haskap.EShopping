using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Infra;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.Catalog.Module;

public class CatalogModule : BaseModule<CatalogModule>, ICatalogModule
{
    public CatalogModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            Application.DependencyInjection.AddApplication(services, configuration);
            Application.Backoffice.DependencyInjection.AddApplication(services, configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            services.RegisterHandlersFromAssembly(typeof(Application.Backoffice.DependencyInjection).Assembly);

            return services;
        }
    }
}
