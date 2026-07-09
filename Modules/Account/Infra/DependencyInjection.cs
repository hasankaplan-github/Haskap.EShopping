using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Account.Domain;
using Modules.Account.Domain.AccountAggregate;
using Modules.Account.Domain.ExternalServices;
using Modules.Account.Infra.Db.Contexts.AccountDbContext;
using Modules.Account.Infra.ExternalServices;
using Modules.AuditLog.Infra.Db.Interceptors;

namespace Modules.Account.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        services.AddHttpClient<IGoogleReCaptchaService, GoogleReCaptchaService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
        });


        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddMyDbContextFactory<IAccountDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder => 
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "account");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddAuditLogInterceptors(serviceProvider);

            options.UseSeeding((context, _) =>
            {
                var accountExists = context.Set<Domain.AccountAggregate.Account>().Any();

                if (accountExists)
                {
                    return;
                }

                var hashProvider = serviceProvider.GetRequiredService<IHashProvider>();

                var password = new Password("Admin123.", Salt.Generate(), hashProvider);
                var credentials = new Credentials("Admin", password);

                var hostAccount = new Domain.AccountAggregate.Account(
                    GuidGenerator.CreateSimpleGuid(),
                    "Admin",
                    "User",
                    null,
                    null,
                    credentials,
                    context.Set<Domain.AccountAggregate.Account>());

                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin);
                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Read);
                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Update);

                context.Set<Domain.AccountAggregate.Account>().Add(hostAccount);
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                var accountExists = await context.Set<Domain.AccountAggregate.Account>().AnyAsync(cancellationToken);

                if (accountExists)
                {
                    return;
                }

                var hashProvider = serviceProvider.GetRequiredService<IHashProvider>();

                var password = new Password("Admin123.", Salt.Generate(), hashProvider);
                var credentials = new Credentials("Admin", password);

                var hostAccount = new Domain.AccountAggregate.Account(
                    GuidGenerator.CreateSimpleGuid(),
                    "Admin",
                    "User",
                    null,
                    null,
                    credentials,
                    context.Set<Domain.AccountAggregate.Account>());

                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin);
                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Read);
                hostAccount.AddPermission(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Update);

                context.Set<Domain.AccountAggregate.Account>().Add(hostAccount);
                await context.SaveChangesAsync(cancellationToken);
            });
        });

        return services;
    }
}
