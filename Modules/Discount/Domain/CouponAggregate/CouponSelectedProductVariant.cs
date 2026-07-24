using Haskap.DddBase.Domain;

namespace Modules.Discount.Domain.CouponAggregate;

public class CouponSelectedProductVariant : Entity
{
    public Guid CouponId { get; private set; }
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }

    private CouponSelectedProductVariant()
    {}

    public CouponSelectedProductVariant(Guid id)
        : base(id)
    {}
}
