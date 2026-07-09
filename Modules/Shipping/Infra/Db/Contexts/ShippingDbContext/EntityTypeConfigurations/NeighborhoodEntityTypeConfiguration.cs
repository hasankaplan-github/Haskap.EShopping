using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shipping.Domain.NeighborhoodAggregate;

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.EntityTypeConfigurations;

public class NeighborhoodEntityTypeConfiguration : BaseEntityTypeConfiguration<Neighborhood>
{
    public override void Configure(EntityTypeBuilder<Neighborhood> builder)
    {
        base.Configure(builder);
    }
}
