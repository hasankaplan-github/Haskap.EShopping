using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.Discount.Domain;
using Modules.Discount.Domain.RegularCouponAggregate;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IDiscountDbContext
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

    public DbSet<SpecialCoupon> SpecialCoupon { get; set; }
    public DbSet<RegularCoupon> RegularCoupon { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("discount");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("DiscountDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }
}
