using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class SpecialCouponCategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<SpecialCouponCategory>
{
    public override void Configure(EntityTypeBuilder<SpecialCouponCategory> builder)
    {
        base.Configure(builder);
    }
}
