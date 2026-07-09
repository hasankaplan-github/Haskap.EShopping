using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shipping.Domain.CityAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.EntityTypeConfigurations;

public class CityEntityTypeConfiguration : BaseEntityTypeConfiguration<City>
{
    public override void Configure(EntityTypeBuilder<City> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Districts)
            .WithOne()
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
