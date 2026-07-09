using Haskap.DddBase.Domain;
using Haskap.DddBase.Infra;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Haskap.DddBase.Utilities.Module;
using Haskap.EShopping.Domain.Providers;
using Haskap.EShopping.Infra.Providers;
using Haskap.EShopping.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Modules.Account.Module;
using Modules.AuditLog.Module;
using Modules.Basket.Module;
using Modules.Catalog.Module;
using Modules.CustomMessage.Module;
using Modules.Discount.Module;
using Modules.Email.Module;
using Modules.GlobalExceptionHandling.Module;
using Modules.ModuleManagement.Module;
using Modules.Order.Module;
using Modules.Shipping.Module;

namespace Haskap.EShopping.Ui.MvcWebUi;

public static class DependencyInjection
{
    private const string ConnectionStringName = "HaskapEShoppingConnectionString";

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModule<ModuleManagementModule.Registrar>(configuration, ConnectionStringName, "ModuleManagementMigrations");
        services.AddModule<AuditLogModule.Registrar>(configuration, ConnectionStringName, "AuditLogMigrations");
        services.AddModule<GlobalExceptionHandlingModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<AccountModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<EmailModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<CustomMessageModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<CatalogModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<BasketModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<DiscountModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<ShippingModule.Registrar>(configuration, ConnectionStringName, null);
        services.AddModule<OrderModule.Registrar>(configuration, ConnectionStringName, null);

        return services;
    }

    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddBaseInfra();

        services.AddSingleton<ICacheKeyProvider, CacheKeyProvider>();
        services.AddScoped<IAnonymousAccountProvider, AnonymousAccountProvider>();

        return services;
    }

    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IPermissionProvider, PermissionProvider>();

        services.AddAuthorization(new PermissionProvider().ConfigureAuthorization);

        services.AddScoped<IAuthorizationHandler, SignInCheckAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }

    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddBaseDomainServices();
    }

    public static void AddHostedServices(this IServiceCollection services)
    {
        services.Configure<HostOptions>(options =>
        {
            options.ServicesStopConcurrently = true;
            options.ServicesStartConcurrently = true;
        });
    }
}
