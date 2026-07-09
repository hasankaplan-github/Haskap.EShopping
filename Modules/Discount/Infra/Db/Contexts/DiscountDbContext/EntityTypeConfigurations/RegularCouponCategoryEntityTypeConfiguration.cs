using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.RegularCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class RegularCouponCategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<RegularCouponCategory>
{
    public override void Configure(EntityTypeBuilder<RegularCouponCategory> builder)
    {
        base.Configure(builder);
    }
}
