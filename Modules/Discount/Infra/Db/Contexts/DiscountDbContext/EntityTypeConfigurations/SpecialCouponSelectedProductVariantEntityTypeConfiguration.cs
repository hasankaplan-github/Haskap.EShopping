using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class SpecialCouponSelectedProductVariantEntityTypeConfiguration : BaseEntityTypeConfiguration<SpecialCouponSelectedProductVariant>
{
    public override void Configure(EntityTypeBuilder<SpecialCouponSelectedProductVariant> builder)
    {
        base.Configure(builder);
    }
}
