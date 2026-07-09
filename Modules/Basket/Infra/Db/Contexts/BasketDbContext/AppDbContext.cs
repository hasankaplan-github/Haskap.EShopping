using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.Basket.Domain;
using Modules.Basket.Domain.BasketAggregate;

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IBasketDbContext
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

    public DbSet<Domain.BasketAggregate.Basket> Basket { get; set; }
    public DbSet<BasketItem> BasketItem { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("basket");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("BasketDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }
}
