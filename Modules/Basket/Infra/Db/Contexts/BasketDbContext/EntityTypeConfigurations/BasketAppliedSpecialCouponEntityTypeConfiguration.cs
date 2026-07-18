using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Basket.Domain.BasketAggregate;

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.EntityTypeConfigurations;

public class BasketAppliedSpecialCouponEntityTypeConfiguration : BaseEntityTypeConfiguration<BasketAppliedSpecialCoupon>
{
    public override void Configure(EntityTypeBuilder<BasketAppliedSpecialCoupon> builder)
    {
        base.Configure(builder);
    }
}
