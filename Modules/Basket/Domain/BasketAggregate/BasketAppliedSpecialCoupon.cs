using Haskap.EShopping.Domain;

namespace Modules.Basket.Domain.BasketAggregate;

public class BasketAppliedSpecialCoupon : Entity
{
    public Guid BasketId { get; private set; }
    public Guid SpecialCouponId { get; set; }

    private BasketAppliedSpecialCoupon()
    {}

    public BasketAppliedSpecialCoupon(Guid id)
        : base(id)
    {}
}
