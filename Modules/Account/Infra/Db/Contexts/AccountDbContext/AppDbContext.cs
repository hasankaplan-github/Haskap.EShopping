using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Domain;
using Modules.Account.Domain.AccountAggregate;
using Modules.Account.Domain.RoleAggregate;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IAccountDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterManagerProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("account");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("AccountDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }

    public DbSet<Domain.AccountAggregate.Account> Account { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<AccountRole> AccountRole { get; set; }
    public DbSet<OpenLogin> OpenLogin { get; set; }
}
