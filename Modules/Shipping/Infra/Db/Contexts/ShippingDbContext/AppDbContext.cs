using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.Shipping.Domain;
using Modules.Shipping.Domain.ShippingAddressAggregate;
using Modules.Shipping.Domain.CityAggregate;
using Modules.Shipping.Domain.DistrictAggregate;
using Modules.Shipping.Domain.NeighborhoodAggregate;
using Modules.Shipping.Domain.ShippingFeeAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, IShippingDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(options, currentTenantProvider, globalQueryFilterManagerProvider)
    {
    }

    public DbSet<City> City { get; set; }
    public DbSet<District> District { get; set; }
    public DbSet<Neighborhood> Neighborhood { get; set; }
    public DbSet<ShippingAddress> ShippingAddress { get; set; }
    public DbSet<ShippingFee> ShippingFee { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("shipping");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("ShippingDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }
}
