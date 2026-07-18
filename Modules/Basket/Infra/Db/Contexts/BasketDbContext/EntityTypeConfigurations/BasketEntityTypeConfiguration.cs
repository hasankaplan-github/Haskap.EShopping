using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.EntityTypeConfigurations;

public class BasketEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.BasketAggregate.Basket>
{
    public override void Configure(EntityTypeBuilder<Domain.BasketAggregate.Basket> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AppliedSpecialCoupons)
            .WithOne()
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RemovedRegularCoupons)
            .WithOne()
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
