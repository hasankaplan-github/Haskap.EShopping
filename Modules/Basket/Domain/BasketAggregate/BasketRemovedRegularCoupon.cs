using Haskap.EShopping.Domain;

namespace Modules.Basket.Domain.BasketAggregate;

public class BasketRemovedRegularCoupon : Entity
{
    public Guid BasketId { get; private set; }
    public Guid RegularCouponId { get; set; }

    private BasketRemovedRegularCoupon()
    {}

    public BasketRemovedRegularCoupon(Guid id)
        : base(id)
    {}
}
