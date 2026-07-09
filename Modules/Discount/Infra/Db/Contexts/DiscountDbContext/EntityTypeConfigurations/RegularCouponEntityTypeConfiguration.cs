using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.RegularCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class RegularCouponEntityTypeConfiguration : CouponEntityTypeConfiguration<RegularCoupon>
{
    public override void Configure(EntityTypeBuilder<RegularCoupon> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Categories)
            .WithOne()
            .HasForeignKey(x => x.RegularCouponId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SelectedProductVariants)
            .WithOne()
            .HasForeignKey(x => x.RegularCouponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
