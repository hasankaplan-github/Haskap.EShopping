using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Domain.Shared.Consts;

namespace Modules.Catalog.Domain;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDomain(IConfiguration configuration)
        {
            services.AddOptions<VariantPhotoSettings>().BindConfiguration(VariantPhotoSettings.SectionName);

            return services;
        }
    }
}
