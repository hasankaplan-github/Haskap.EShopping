using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Discount.Domain.Common;

public class Discount : ValueObject
{
    public bool IsFreeShippingDiscount { get; private set; }
    public int? QuantityDivider { get; private set; } = null;
    public decimal? FixedPriceValue{ get; private set; } = null;
    public decimal? FixedDiscountAmountValue { get; private set; } = null;
    public decimal? PercentageDiscountValue { get; private set; } = null;

    private Discount()
    {}

    public static Discount CreateFreeShippingDiscount()
    {
        return new Discount
        {
            IsFreeShippingDiscount = true
        };
    }

    public static Discount CreateFixedPriceDividerDiscount(int quantityDivider, decimal fixedPriceValue)
    {
        Guard.Against.NegativeOrZero(quantityDivider, nameof(quantityDivider));

        return new Discount
        {
            QuantityDivider = quantityDivider,
            FixedPriceValue = fixedPriceValue
        };
    }

    public static Discount CreateFixedDiscountAmountDividerDiscount(int quantityDivider, decimal fixedDiscountAmountValue)
    {
        Guard.Against.NegativeOrZero(quantityDivider, nameof(quantityDivider));

        return new Discount
        {
            QuantityDivider = quantityDivider,
            FixedDiscountAmountValue = fixedDiscountAmountValue
        };
    }

    public static Discount CreatePercentageDiscountDividerDiscount(int quantityDivider, decimal percentageDiscountValue)
    {
        Guard.Against.NegativeOrZero(quantityDivider, nameof(quantityDivider));
        Guard.Against.OutOfRange(percentageDiscountValue, nameof(percentageDiscountValue), 0, 100);

        return new Discount
        {
            QuantityDivider = quantityDivider,
            PercentageDiscountValue = percentageDiscountValue
        };
    }

    public static Discount CreateFixedDiscountAmountDiscount(decimal fixedDiscountAmountValue)
    {
        return new Discount
        {
            FixedDiscountAmountValue = fixedDiscountAmountValue
        };
    }

    public static Discount CreatePercentageDiscount(decimal percentageDiscountValue)
    {
        Guard.Against.OutOfRange(percentageDiscountValue, nameof(percentageDiscountValue), 0, 100);

        return new Discount
        {
            PercentageDiscountValue = percentageDiscountValue
        };
    }

    public Money GetDiscountAmount(decimal minPriceValue, int totalQuantity)
    {
        if (IsFreeShippingDiscount)
        {
            return Money.Zero;
        }

        decimal discountAmountValue = 0;

        if (QuantityDivider.HasValue)
        {
            var discountedCount = (int)Math.Floor((decimal)totalQuantity / QuantityDivider.Value);

            if (FixedPriceValue.HasValue)
            {
                discountAmountValue = discountedCount * (minPriceValue - FixedPriceValue.Value);
            }
            else if (FixedDiscountAmountValue.HasValue)
            {
                discountAmountValue = discountedCount * FixedDiscountAmountValue.Value;
            }
            else
            {
                discountAmountValue = discountedCount * ((minPriceValue * PercentageDiscountValue!.Value) / 100);
            }
        }
        else
        {
            if (FixedDiscountAmountValue.HasValue)
            {
                discountAmountValue = FixedDiscountAmountValue.Value;
            }
            else
            {
                discountAmountValue = (minPriceValue * PercentageDiscountValue!.Value) / 100;
            }
        }

        return new Money(discountAmountValue);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return QuantityDivider ?? 0;
        yield return FixedPriceValue ?? 0;
        yield return FixedDiscountAmountValue ?? 0;
        yield return PercentageDiscountValue ?? 0;
    }
}
