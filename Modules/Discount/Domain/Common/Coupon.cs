using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Discount.Domain.Common;

public class Coupon : AggregateRoot, IIsActive
{
    public string? Description { get; set; }
    public Money BasketMinTotalAmount { get; private set; }
    public UsageCount UsageCount { get; private set; }
    public Common.Discount Discount { get; private set; }
    public DateRange DateRange { get; private set; }
    public bool IsActive { get; set; }

    protected Coupon()
    { }

    public Coupon(
        Guid id,
        string? description,
        Money basketMinTotalAmount,
        UsageCount usageCount,
        Common.Discount discount,
        DateRange dateRange,
        bool isActive)
        : base(id)
    {
        Description = description;
        SetBasketMinTotalAmount(basketMinTotalAmount);
        SetUsageCount(usageCount);
        SetDiscount(discount);
        SetDateRange(dateRange);
        IsActive = isActive;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetBasketMinTotalAmount(Money basketMinTotalAmount)
    {
        Guard.Against.Null(basketMinTotalAmount, nameof(basketMinTotalAmount));
        Guard.Against.Negative(basketMinTotalAmount.Value, nameof(basketMinTotalAmount));

        BasketMinTotalAmount = basketMinTotalAmount;
    }

    public void SetUsageCount(UsageCount usageCount)
    {
        Guard.Against.Null(usageCount, nameof(usageCount));
     
        UsageCount = usageCount;
    }

    public void SetDiscount(Common.Discount discount)
    {
        Guard.Against.Null(discount, nameof(discount));

        Discount = discount;
    }

    public void SetDateRange(DateRange dateRange)
    {
        Guard.Against.Null(dateRange, nameof(dateRange));

        DateRange = dateRange;
    }
}
