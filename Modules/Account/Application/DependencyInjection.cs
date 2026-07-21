using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Account.Application.Account;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Contracts.Role;
using Modules.Account.Application.Role;

namespace Modules.Account.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IRoleService, RoleService>();

        return services;
    }
}
