using Haskap.DddBase.Domain.Specifications;
using Haskap.EShopping.Domain.Common;
using System.Linq.Expressions;

namespace Modules.Discount.Domain.RegularCouponAggregate.Specifications;

public class ValidRegularCouponsForBasketSpecification : Specification<RegularCoupon>
{
    private IList<Guid> _categoryIds;
    private IList<(Guid ProductId, Guid VariantId)> _productVariantIds;
    private Money _basketTotalAmount;

    public ValidRegularCouponsForBasketSpecification(
        IList<Guid> categoryIds,
        IList<(Guid ProductId, Guid VariantId)> productVariantIds,
        Money basketTotalAmount)
    {
        _categoryIds = categoryIds;
        _productVariantIds = productVariantIds;
        _basketTotalAmount = basketTotalAmount;
    }

    public override Expression<Func<RegularCoupon, bool>> ToExpression()
    {
        var utcNow = DateTime.UtcNow;

        Expression<Func<RegularCoupon, bool>> expression = x =>
            x.UsageCount.Value < x.UsageCount.Limit &&
            utcNow >= x.DateRange.UtcStartDateTime && utcNow <= x.DateRange.UtcEndDateTime &&
            x.BasketMinTotalAmount.Value <= _basketTotalAmount.Value &&
            (x.Categories.Count == 0 || x.Categories.Any(c => _categoryIds.Contains(c.CategoryId)));

        Expression<Func<RegularCoupon, bool>> orExpressionsForSelectedProductVariants = x => x.SelectedProductVariants.Count == 0;
        foreach (var productVariantId in _productVariantIds)
        {
            Expression<Func<RegularCoupon, bool>> otherExpression = x => x.SelectedProductVariants.Any(y => y.ProductId == productVariantId.ProductId && y.VariantId == productVariantId.VariantId);
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
