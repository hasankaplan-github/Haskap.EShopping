using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain.CategoryAggregate;
using Modules.Catalog.Domain.ProductAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class ProductCategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>()
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
