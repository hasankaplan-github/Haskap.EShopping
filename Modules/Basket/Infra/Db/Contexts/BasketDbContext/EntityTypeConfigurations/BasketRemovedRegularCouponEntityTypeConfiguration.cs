using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Basket.Domain.BasketAggregate;

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.EntityTypeConfigurations;

public class BasketRemovedRegularCouponEntityTypeConfiguration : BaseEntityTypeConfiguration<BasketRemovedRegularCoupon>
{
    public override void Configure(EntityTypeBuilder<BasketRemovedRegularCoupon> builder)
    {
        base.Configure(builder);
    }
}
