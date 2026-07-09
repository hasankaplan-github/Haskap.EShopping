using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Domain.Shared.Consts;

namespace Modules.Catalog.Application.Backoffice;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication(IConfiguration configuration)
        {
            services.AddOptions<VariantPhotoSettings>().BindConfiguration(VariantPhotoSettings.SectionName);

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISizeAttributeService, SizeAttributeService>();
            services.AddTransient<IColorAttributeService, ColorAttributeService>();

            return services;
        }
    }
}
