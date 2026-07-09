using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.Order.Domain;

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, IOrderDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(options, currentTenantProvider, globalQueryFilterManagerProvider)
    {
    }

    public DbSet<Domain.OrderAggregate.Order> Order { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("order");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("OrderDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }
}
