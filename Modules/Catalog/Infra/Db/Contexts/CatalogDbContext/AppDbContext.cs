using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext.EntityTypeConfigurations;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.CategoryAggregate;
using Modules.Catalog.Domain.ProductAggregate;
using Modules.Catalog.Domain.ColorAttributeAggregate;
using Modules.Catalog.Domain.SizeAttributeAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext;

public class AppDbContext : BaseEfCoreNpgsqlDbContext, ICatalogDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(options, currentTenantProvider, globalQueryFilterManagerProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("catalog");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("CatalogDbContext"));
        builder.ApplyConfiguration(new AuditHistoryLogEntityTypeConfigurationExcluded());

        base.OnModelCreating(builder);
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<ProductVariant> ProductVariant { get; set; }
    public DbSet<ColorAttribute> ColorAttribute { get; set; }
    public DbSet<SizeAttribute> SizeAttribute { get; set; }
}
