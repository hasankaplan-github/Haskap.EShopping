using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shipping.Domain.ShippingAddressAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.EntityTypeConfigurations;

public class ShippingAddressEntityTypeConfiguration : BaseEntityTypeConfiguration<ShippingAddress>
{
    public override void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.OwnerAccountId);

        builder.HasOne(x => x.City)
            .WithMany()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.District)
            .WithMany()
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Neighborhood)
            .WithMany()
            .HasForeignKey(x => x.NeighborhoodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
