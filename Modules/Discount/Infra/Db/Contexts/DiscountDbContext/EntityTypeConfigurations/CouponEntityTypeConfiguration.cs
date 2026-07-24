using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.CouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class CouponEntityTypeConfiguration : BaseEntityTypeConfiguration<Coupon>
{
    public override void Configure(EntityTypeBuilder<Coupon> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Categories)
            .WithOne()
            .HasForeignKey(x => x.CouponId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SelectedProductVariants)
            .WithOne()
            .HasForeignKey(x => x.CouponId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.BasketMinTotalAmount, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });

        builder.OwnsOne(x => x.UsageCount);

        builder.OwnsOne(x => x.Discount);

        builder.OwnsOne(x => x.DateRange);
    }
}
