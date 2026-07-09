using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shipping.Domain.DistrictAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.EntityTypeConfigurations;

public class DistrictEntityTypeConfiguration : BaseEntityTypeConfiguration<District>
{
    public override void Configure(EntityTypeBuilder<District> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Neighborhoods)
            .WithOne()
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
