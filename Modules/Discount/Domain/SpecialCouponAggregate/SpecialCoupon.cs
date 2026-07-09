using Ardalis.GuardClauses;
using Haskap.EShopping.Domain.Common;
using Modules.Discount.Domain.Common;

namespace Modules.Discount.Domain.SpecialCouponAggregate;

public class SpecialCoupon : Coupon
{
    public string Code { get; private set; }
    public IReadOnlyList<SpecialCouponCategory> Categories => _categories.AsReadOnly();
    private List<SpecialCouponCategory> _categories = [];
    public IReadOnlyList<SpecialCouponSelectedProductVariant> SelectedProductVariants => _selectedProductVariants.AsReadOnly();
    private List<SpecialCouponSelectedProductVariant> _selectedProductVariants = [];

    private SpecialCoupon()
    {}

    public SpecialCoupon(
        Guid id,
        string code,
        string? description,
        Money basketMinTotalAmount,
        UsageCount usageCount,
        Common.Discount discount,
        DateRange dateRange,
        bool isActive)
        : base(id, description, basketMinTotalAmount, usageCount, discount, dateRange, isActive)
    {
        SetCode(code);
    }

    public bool IsValid(IList<Guid> categoryIds, IList<(Guid productId, Guid variantId)> productVariantIds, Money basketTotalAmount)
    {
        return
            IsActive &&
            UsageCount.CanIncrement() &&
            DateRange.IsInRange(DateTime.UtcNow) &&
            BasketMinTotalAmount.Value <= basketTotalAmount.Value &&
            (_categories.Count == 0 || _categories.Any(c => categoryIds.Contains(c.CategoryId))) &&
            (_selectedProductVariants.Count == 0 || _selectedProductVariants.Any(p => productVariantIds.Contains((p.ProductId, p.VariantId))));
    }

    public void SetCode(string code)
    {
        Guard.Against.NullOrWhiteSpace(code, nameof(code));

        Code = code;
    }
}
