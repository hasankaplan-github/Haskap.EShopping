using Haskap.EShopping.Domain;

namespace Modules.Discount.Domain.SpecialCouponAggregate;

public class SpecialCouponCategory : Entity
{
    public Guid SpecialCouponId { get; private set; }
    public Guid CategoryId { get; set; }

    private SpecialCouponCategory()
    { }

    public SpecialCouponCategory(Guid id)
        : base(id)
    {}
}
