using Haskap.EShopping.Domain;

namespace Modules.Discount.Domain.SpecialCouponAggregate;

public class SpecialCouponSelectedProductVariant : Entity
{
    public Guid SpecialCouponId { get; private set; }
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }

    private SpecialCouponSelectedProductVariant()
    {}

    public SpecialCouponSelectedProductVariant(Guid id)
        : base(id)
    {}
}
