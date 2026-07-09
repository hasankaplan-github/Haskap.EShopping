using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Discount.Application;
using Modules.Discount.Application.Contracts;
using Modules.Discount.Infra;
using Modules.ModuleManagement.Application.Contracts.Module;

namespace Modules.Discount.Module;

public class DiscountModule : BaseModule<DiscountModule>, IDiscountModule
{
    public DiscountModule(
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
