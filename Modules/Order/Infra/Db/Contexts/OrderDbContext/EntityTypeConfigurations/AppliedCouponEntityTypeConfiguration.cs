using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Order.Domain.OrderAggregate;

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext.EntityTypeConfigurations;

internal class AppliedCouponEntityTypeConfiguration : BaseEntityTypeConfiguration<AppliedCoupon>
{
    public override void Configure(EntityTypeBuilder<AppliedCoupon> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.DiscountAmount, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });
    }
}
