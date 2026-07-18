using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain.ProductAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class ProductEntityTypeConfiguration : BaseEntityTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.OwnsOne(x => x.Sku, skuBuilder =>
        {
            skuBuilder.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsMany(x => x.AttributeTypePairForDetails, pairBuilder =>
        {
            pairBuilder.WithOwner().HasForeignKey("ProductId");
            pairBuilder.HasKey(x => x.Id);
            pairBuilder.Property(x => x.Id).ValueGeneratedNever();

            pairBuilder.Property(y => y.AttributeType).HasConversion<string>();
        });

        builder.HasMany(x => x.Variants)
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(b => b.SearchVectorTurkish)
            .HasComputedColumnSql(
                @"to_tsvector('turkish', ""name"" || ' ' || ""description"" || ' ' || ""sku_value"")",
                stored: true);

        builder.HasIndex(b => b.SearchVectorTurkish)
            .HasMethod("GIN");
    }
}
