using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain.ProductAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class ProductSearchViewEntityTypeConfiguration : BaseEntityTypeConfiguration<ProductSearchView>
{
    public override void Configure(EntityTypeBuilder<ProductSearchView> builder)
    {
        builder.HasNoKey(); // Declare as keyless if it doesn't have a unique ID, or use .HasKey(x => x.Id)
        builder.ToView("product_search_view"); // Map to your DB view name

        // Explicitly tell EF Core that SearchVector maps to a tsvector type
        builder.Property(x => x.SearchVectorTurkish).HasColumnType("tsvector");

        //builder.HasIndex(b => b.SearchVectorTurkish).HasMethod("GIN");
    }
}
