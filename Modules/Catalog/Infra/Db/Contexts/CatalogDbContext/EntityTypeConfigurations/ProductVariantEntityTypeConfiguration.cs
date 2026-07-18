using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain.ProductAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class ProductVariantEntityTypeConfiguration : BaseEntityTypeConfiguration<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Sku, skuBuilder =>
        {
            skuBuilder.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsOne(x => x.Slug, slugBuilder =>
        {
            slugBuilder.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsMany(x => x.Attributes, attributesBuilder =>
        {
            attributesBuilder.WithOwner().HasForeignKey("ProductVariantId");
            attributesBuilder.HasKey(x => x.Id);
            attributesBuilder.Property(x => x.Id).ValueGeneratedNever();

            attributesBuilder.Property(y => y.AttributeType).HasConversion<string>();

            attributesBuilder.Property(b => b.SearchVectorTurkish)
                .HasComputedColumnSql(
                    @"to_tsvector('turkish', display_name || ' ' || value || ' ' || COALESCE(description, ''))",
                    stored: true);
            attributesBuilder.HasIndex(b => b.SearchVectorTurkish)
                .HasMethod("GIN");
        });

        builder.OwnsMany(x => x.Pictures, picturesBuilder =>
        {
            picturesBuilder.WithOwner().HasForeignKey("ProductVariantId");
            picturesBuilder.HasKey(x => x.Id);
            picturesBuilder.Property(x => x.Id).ValueGeneratedNever();

            picturesBuilder.OwnsOne(x => x.PhotoFile, photoFileBuilder =>
            {
                photoFileBuilder.Ignore(x => x.Id);
            });
        });

        builder.OwnsOne(x => x.OldPrice, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });
        builder.OwnsOne(x => x.Price, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });
        builder.OwnsOne(x => x.StockQuantity);

        builder.Property(b => b.SearchVectorTurkish)
            .HasComputedColumnSql(
                @"to_tsvector('turkish', ""sku_value"")",
                stored: true);

        builder.HasIndex(b => b.SearchVectorTurkish)
            .HasMethod("GIN");

        //builder.HasGeneratedTsVectorColumn(
        //    p => p.SearchVectorTurkish,
        //    "turkish",  // Text search config
        //    p => $"{string.Join(" ", p.Attributes.Select(a => $"{a.DisplayName} {a.Value} {a.Description}"))}")
        //    .HasIndex(p => p.SearchVectorTurkish)
        //    .HasMethod("GIN");
    }
}
