using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Account.Domain.Shared.Consts;

namespace Modules.Account.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GoogleReCaptchaSettings>().BindConfiguration(GoogleReCaptchaSettings.SectionName);

        return services;
    }
}
