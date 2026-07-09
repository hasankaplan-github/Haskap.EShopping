using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Account.Application.Account;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Contracts.Role;
using Modules.Account.Application.Role;
using Modules.Account.Domain.Shared.Consts;

namespace Modules.Account.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GoogleReCaptchaSettings>().BindConfiguration(GoogleReCaptchaSettings.SectionName);

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IRoleService, RoleService>();

        return services;
    }
}
