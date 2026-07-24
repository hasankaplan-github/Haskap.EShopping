using Haskap.DddBase.Domain;

namespace Modules.Discount.Domain.CouponAggregate;

public class CouponCategory : Entity
{
    public Guid CouponId { get; private set; }
    public Guid CategoryId { get; set; }

    private CouponCategory()
    { }

    public CouponCategory(Guid id)
        : base(id)
    {}
}
