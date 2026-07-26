using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Discount.Domain.CouponAggregate;

public class Coupon : AggregateRoot, IIsActive
{
    public string? Code { get; private set; }
    public IReadOnlyList<CouponCategory> Categories => _categories.AsReadOnly();
    private List<CouponCategory> _categories = [];
    public IReadOnlyList<CouponSelectedProductVariant> SelectedProductVariants => _selectedProductVariants.AsReadOnly();
    private List<CouponSelectedProductVariant> _selectedProductVariants = [];
    public string? Description { get; set; }
    public Money BasketMinTotalAmount { get; private set; }
    public UsageCount UsageCount { get; private set; }
    public Discount Discount { get; private set; }
    public DateRange DateRange { get; private set; }
    public bool IsActive { get; set; }

    private Coupon()
    { }

    public Coupon(
        Guid id,
        string? description,
        Money basketMinTotalAmount,
        UsageCount usageCount,
        Discount discount,
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

    public bool IsValid(IList<Guid> categoryIds, IList<(Guid productId, Guid variantId)> productVariantIds, Money basketTotalAmount, TimeProvider timeProvider)
    {
        return
            IsActive &&
            UsageCount.CanIncrement() &&
            DateRange.IsInRange(timeProvider.GetUtcNow().DateTime) &&
            BasketMinTotalAmount.Value <= basketTotalAmount.Value &&
            (_categories.Count == 0 || _categories.Any(c => categoryIds.Contains(c.CategoryId))) &&
            (_selectedProductVariants.Count == 0 || _selectedProductVariants.Any(p => productVariantIds.Contains((p.ProductId, p.VariantId))));
    }

    public void SetCode(string code)
    {
        Guard.Against.NullOrWhiteSpace(code, nameof(code));

        Code = code;
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

    public void SetDiscount(Discount discount)
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
