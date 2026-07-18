using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Domain;

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.EntityTypeConfigurations;

public class AttributeBaseEntityTypeConfiguration<TEntity> : BaseEntityTypeConfiguration<TEntity>
    where TEntity : AttributeBase
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);
    }
}
