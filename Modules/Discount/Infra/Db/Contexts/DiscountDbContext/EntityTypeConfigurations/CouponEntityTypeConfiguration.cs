using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.Common;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class CouponEntityTypeConfiguration<TEntity> : BaseEntityTypeConfiguration<TEntity>
    where TEntity : Coupon
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.BasketMinTotalAmount, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });

        builder.OwnsOne(x => x.UsageCount);

        builder.OwnsOne(x => x.Discount);

        builder.OwnsOne(x => x.DateRange);
    }
}
