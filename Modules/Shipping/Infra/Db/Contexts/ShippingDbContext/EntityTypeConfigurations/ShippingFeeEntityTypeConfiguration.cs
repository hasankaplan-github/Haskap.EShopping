using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shipping.Domain.ShippingFeeAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.EntityTypeConfigurations;

public class ShippingFeeEntityTypeConfiguration : BaseEntityTypeConfiguration<ShippingFee>
{
    public override void Configure(EntityTypeBuilder<ShippingFee> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Price, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });
    }
}
