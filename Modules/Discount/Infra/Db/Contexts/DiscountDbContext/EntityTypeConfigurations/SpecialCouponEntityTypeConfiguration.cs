using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class SpecialCouponEntityTypeConfiguration : CouponEntityTypeConfiguration<SpecialCoupon>
{
    public override void Configure(EntityTypeBuilder<SpecialCoupon> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Categories)
            .WithOne()
            .HasForeignKey(x => x.SpecialCouponId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SelectedProductVariants)
            .WithOne()
            .HasForeignKey(x => x.SpecialCouponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
