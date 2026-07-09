using Haskap.EShopping.Domain;

namespace Modules.Discount.Domain.RegularCouponAggregate;

public class RegularCouponSelectedProductVariant : Entity
{
    public Guid RegularCouponId { get; private set; }
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }

    private RegularCouponSelectedProductVariant()
    {}

    public RegularCouponSelectedProductVariant(Guid id)
        : base(id)
    {}
}
