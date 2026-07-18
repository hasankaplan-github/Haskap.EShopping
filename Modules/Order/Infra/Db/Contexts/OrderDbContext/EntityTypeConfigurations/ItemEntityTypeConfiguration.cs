using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Order.Domain.OrderAggregate;

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext.EntityTypeConfigurations;

internal class ItemEntityTypeConfiguration : BaseEntityTypeConfiguration<Item>
{
    public override void Configure(EntityTypeBuilder<Item> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.Price, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });

        builder.OwnsOne(x => x.Total, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });

        builder.OwnsOne(x => x.TotalWithDiscount, x =>
        {
            x.Property(y => y.Currency).HasConversion<string>();
        });
    }
}
