using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain.CategoryAggregate;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class CategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.OwnsOne(x => x.Slug);
    }
}
