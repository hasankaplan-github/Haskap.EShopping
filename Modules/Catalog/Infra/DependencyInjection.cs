using Haskap.DddBase.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Infra.Db.Interceptors;
using Modules.Catalog.Domain;
using Modules.Catalog.Infra.Db.Contexts.CatalogDbContext;

namespace Modules.Catalog.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddMyDbContextFactory<ICatalogDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder => 
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "catalog");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddAuditLogInterceptors(serviceProvider);
        });

        //services.AddScoped<ICancellableGlobalQueryFilterProvider, CancellableGlobalQueryFilterProvider>();
        //services.AddScoped<IHasMaintenanceModeGlobalQueryFilterProvider, HasMaintenanceModeGlobalQueryFilterProvider>();

        return services;
    }
}
