using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Events;
using Haskap.DddBase.Utilities.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Account.Application;
using Modules.Account.Domain.Shared;
using Modules.Account.Infra;
using Modules.ModuleManagement.Application.Contracts;

namespace Modules.Account.Module;

public class AccountModule : BaseModule<AccountModule>, IAccountModule
{
    public AccountModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
        : base(moduleService, currentTenantProvider)
    {
    }

    public class Registrar : IModuleRegistrar
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddApplication(configuration);
            services.AddInfra(configuration, connectionStringName, migrationAssembly);
            services.RegisterHandlersFromAssembly(typeof(Application.DependencyInjection).Assembly);
            return services;
        }
    }
}