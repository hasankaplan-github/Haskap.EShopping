using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Discount.Domain.CouponAggregate;

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.EntityTypeConfigurations;

public class CouponCategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<CouponCategory>
{
    public override void Configure(EntityTypeBuilder<CouponCategory> builder)
    {
        base.Configure(builder);
    }
}
