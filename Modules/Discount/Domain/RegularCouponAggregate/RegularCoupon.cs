using Haskap.EShopping.Domain.Common;
using Modules.Discount.Domain.Common;

namespace Modules.Discount.Domain.RegularCouponAggregate;

public class RegularCoupon : Coupon
{
    public IReadOnlyList<RegularCouponCategory> Categories => _categories.AsReadOnly();
    private List<RegularCouponCategory> _categories = [];
    public IReadOnlyList<RegularCouponSelectedProductVariant> SelectedProductVariants => _selectedProductVariants.AsReadOnly();
    private List<RegularCouponSelectedProductVariant> _selectedProductVariants = [];

    private RegularCoupon()
    {}

    public RegularCoupon(
        Guid id,
        string? description,
        Money basketMinTotalAmount,
        UsageCount usageCount,
        Common.Discount discount,
        DateRange dateRange,
        bool isActive)
        : base(id, description, basketMinTotalAmount, usageCount, discount, dateRange, isActive)
    {}

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
}
