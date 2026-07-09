using Haskap.DddBase.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Infra.Db.Interceptors;
using Modules.Basket.Domain;
using Modules.Basket.Infra.Db.Contexts.BasketDbContext;

namespace Modules.Basket.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddMyDbContextFactory<IBasketDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "basket");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddAuditLogInterceptors(serviceProvider);
        });

        return services;
    }
}
