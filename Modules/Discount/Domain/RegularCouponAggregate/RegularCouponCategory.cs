using Haskap.DddBase.Domain;

namespace Modules.Discount.Domain.RegularCouponAggregate;

public class RegularCouponCategory : Entity
{
    public Guid RegularCouponId { get; private set; }
    public Guid CategoryId { get; set; }

    private RegularCouponCategory()
    { }

    public RegularCouponCategory(Guid id)
        : base(id)
    {}
}
