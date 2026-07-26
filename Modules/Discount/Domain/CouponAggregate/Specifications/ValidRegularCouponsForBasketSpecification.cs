using Haskap.DddBase.Domain.Specifications;
using Haskap.EShopping.Domain.Common;
using System.Linq.Expressions;

namespace Modules.Discount.Domain.CouponAggregate.Specifications;

public class ValidRegularCouponsForBasketSpecification : Specification<Coupon>
{
    private IList<Guid> _categoryIds;
    private IList<(Guid ProductId, Guid VariantId)> _productVariantIds;
    private Money _basketTotalAmount;
    private readonly TimeProvider _timeProvider;

    public ValidRegularCouponsForBasketSpecification(
        IList<Guid> categoryIds,
        IList<(Guid ProductId, Guid VariantId)> productVariantIds,
        Money basketTotalAmount,
        TimeProvider timeProvider)
    {
        _categoryIds = categoryIds;
        _productVariantIds = productVariantIds;
        _basketTotalAmount = basketTotalAmount;
        _timeProvider = timeProvider;
    }

    public override Expression<Func<Coupon, bool>> ToExpression()
    {
        var utcNow = _timeProvider.GetUtcNow();

        Expression<Func<Coupon, bool>> expression = x =>
            x.Code == null &&
            x.UsageCount.Value < x.UsageCount.Limit &&
            utcNow >= x.DateRange.UtcStartDateTime && utcNow <= x.DateRange.UtcEndDateTime &&
            x.BasketMinTotalAmount.Value <= _basketTotalAmount.Value &&
            (x.Categories.Count == 0 || x.Categories.Any(c => _categoryIds.Contains(c.CategoryId)));

        Expression<Func<Coupon, bool>> orExpressionsForSelectedProductVariants = x => x.SelectedProductVariants.Count == 0;
        foreach (var productVariantId in _productVariantIds)
        {
            Expression<Func<Coupon, bool>> otherExpression = x => x.SelectedProductVariants.Any(y => y.ProductId == productVariantId.ProductId && y.VariantId == productVariantId.VariantId);
            orExpressionsForSelectedProductVariants = orExpressionsForSelectedProductVariants.Or(otherExpression);
        }

        expression = expression.And(orExpressionsForSelectedProductVariants);

        return expression;

        //return x =>
        //    x.UsageCount.CanIncrement() &&
        //    x.DateRange.IsInRange(DateTime.UtcNow) &&
        //    x.BasketMinTotalAmount.Value <= _basketTotalAmount.Value &&
        //    (x.Categories.Count == 0 || x.Categories.Any(c => _categoryIds.Contains(c.CategoryId))) &&
        //    (x.SelectedProductVariants.Count == 0 || x.SelectedProductVariants.Any(p => _productVariantIds.Any(v => v.ProductId == p.ProductId && v.VariantId == p.VariantId)));
    }
}
