using Haskap.DddBase.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Infra.Db.Interceptors;
using Modules.Order.Domain;
using Modules.Order.Infra.Db.Contexts.OrderDbContext;

namespace Modules.Order.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddMyDbContextFactory<IOrderDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "order");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddAuditLogInterceptors(serviceProvider);
        });

        return services;
    }
}
