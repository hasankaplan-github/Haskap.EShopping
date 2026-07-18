using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Order.Domain.OrderAggregate;

public class AppliedCoupon : Entity
{
    public Guid OrderId { get; private set; }
    public Guid OwnerCouponId { get; private set; }
    public string? Description { get; private set; }
    public Money DiscountAmount { get; private set; }
    public bool IsSpecialCoupon { get; private set; }
    public string? Code { get; private set; } = null;

    private AppliedCoupon()
    {}

    public AppliedCoupon(Guid id, Guid ownerCouponId, string? description, Money discountAmount, bool isSpecialCoupon, string? code)
        : base(id)
    {
        OwnerCouponId = ownerCouponId;
        Description = description;
        DiscountAmount = discountAmount;
        IsSpecialCoupon = isSpecialCoupon;
        Code = code;
    }
}
