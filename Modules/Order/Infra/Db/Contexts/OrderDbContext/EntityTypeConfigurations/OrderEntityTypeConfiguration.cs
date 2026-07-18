using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext.EntityTypeConfigurations;

internal class OrderEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.OrderAggregate.Order>
{
    public override void Configure(EntityTypeBuilder<Domain.OrderAggregate.Order> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.IdempotencyKey).IsUnique();

        builder.OwnsOne(x => x.Code);
        builder.OwnsOne(x => x.Account);
        builder.OwnsOne(x => x.ShippingAddress);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AppliedCoupons)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.ShippingFee, x =>
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
