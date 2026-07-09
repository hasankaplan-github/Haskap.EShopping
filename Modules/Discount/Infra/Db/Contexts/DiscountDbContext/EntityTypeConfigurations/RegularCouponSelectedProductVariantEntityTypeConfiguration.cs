using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.RegularCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class RegularCouponSelectedProductVariantEntityTypeConfiguration : BaseEntityTypeConfiguration<RegularCouponSelectedProductVariant>
{
    public override void Configure(EntityTypeBuilder<RegularCouponSelectedProductVariant> builder)
    {
        base.Configure(builder);
    }
}
