using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.EntityTypeConfigurations;

public class BasketItemEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.BasketAggregate.BasketItem>
{
    public override void Configure(EntityTypeBuilder<Domain.BasketAggregate.BasketItem> builder)
    {
        base.Configure(builder);
    }
}
