using Haskap.DddBase.Application;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Application.Dtos;
using Modules.Catalog.Application.Dtos;
using Modules.Discount.Application.Contracts;
using Modules.Discount.Application.Dtos;
using Modules.Discount.Domain;
using Modules.Discount.Domain.Common;
using Modules.Discount.Domain.RegularCouponAggregate;
using Modules.Discount.Domain.RegularCouponAggregate.Specifications;
using Modules.Discount.Domain.SpecialCouponAggregate;

namespace Modules.Discount.Application;

public class DiscountService : UseCaseService, IDiscountService
{
    private readonly IDiscountDbContext _discountDbContext;

    public DiscountService(IDiscountDbContext discountDbContext)
    {
        _discountDbContext = discountDbContext;
    }

    private async Task ApplyRegularCouponsAsync(BasketOutputDto basketOutput, CancellationToken cancellationToken = default)
    {
        var allCategoryIds = basketOutput.Items.SelectMany(x => x.CategoryIds).Distinct().ToList();
        var allProductVariantIds = basketOutput.Items.Select(x => (x.ProductId, x.VariantId)).ToList();
        var basketTotalAmount = new Money(basketOutput.Total.Value);

        var possibleRegularCoupons = await _discountDbContext.RegularCoupon
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.SelectedProductVariants)
            //.Where(x => x.IsValid(allCategoryIds, allProductVariantIds, basketTotalAmount))
            .Where(new ValidRegularCouponsForBasketSpecification(allCategoryIds, allProductVariantIds, basketTotalAmount))
            .ToListAsync(cancellationToken);

        List<(List<ItemForBasketOutputDto> BasketItems, RegularCoupon PossibleRegularCoupon, Money PossibleDiscountAmount)> discountElements = [];
        foreach (var possibleRegularCoupon in possibleRegularCoupons)
        {
            var basketItems = basketOutput.Items
                .Where(x => possibleRegularCoupon.IsValid(x.CategoryIds, [(x.ProductId, x.VariantId)], basketTotalAmount))
                .ToList();

            var minPriceValue = basketItems.Select(x => x.Price.Value).Min();
            var totalQuantity = basketItems.Sum(x => x.Quantity);
            var possibleDiscountAmount = possibleRegularCoupon.Discount.GetDiscountAmount(minPriceValue, totalQuantity);

            if (possibleDiscountAmount == Money.Zero && !possibleRegularCoupon.Discount.IsFreeShippingDiscount)
            {
                continue;
            }

            discountElements.Add((basketItems, possibleRegularCoupon, possibleDiscountAmount));
        }

        foreach (var discountElement in discountElements.OrderByDescending(x => x.PossibleDiscountAmount.Value))
        {
            if (discountElement.PossibleRegularCoupon.Discount.IsFreeShippingDiscount)
            {
                basketOutput.HasFreeShippingCoupon = true;
                continue;
            }

            var totalValueWithDiscount = discountElement.BasketItems.Sum(x => x.TotalWithDiscount.Value);
            var couponIsApplied = true;
            if (totalValueWithDiscount == 0 ||
                basketOutput.RemovedRegularCouponIds.Contains(discountElement.PossibleRegularCoupon.Id))
            {
                couponIsApplied = false;
            }

            var couponForBasket = new CouponForBasketOutputDto()
            {
                Id = discountElement.PossibleRegularCoupon.Id,
                Description = discountElement.PossibleRegularCoupon.Description,
                DiscountAmount = discountElement.PossibleDiscountAmount.ToMoneyOutputDto(),
                IsApplied = couponIsApplied,
                IsSpecialCoupon = false
            };
            basketOutput.AllPossibleCoupons.Add(couponForBasket);

            if (!couponIsApplied)
            {
                continue;
            }


            var discountAmountValuePerBasketItem = discountElement.PossibleDiscountAmount.Value / discountElement.BasketItems.Count;

            foreach (var basketItem in discountElement.BasketItems)
            {
                var totalValueWithDiscountPerBasketItem = basketItem.TotalWithDiscount.Value - discountAmountValuePerBasketItem;
                if (totalValueWithDiscountPerBasketItem < 0)
                {
                    totalValueWithDiscountPerBasketItem = 0;
                }
                basketItem.TotalWithDiscount = new Money(totalValueWithDiscountPerBasketItem).ToMoneyOutputDto();
            }
        }
    }

    private async Task ApplySpecialCouponsAsync(BasketOutputDto basketOutput, CancellationToken cancellationToken = default)
    {
        var basketTotalAmount = new Money(basketOutput.Total.Value);

        var possibleSpecialCoupons = await _discountDbContext.SpecialCoupon
            .AsNoTracking()
            .Include(x => x.Categories)
            .Include(x => x.SelectedProductVariants)
            .Where(x => basketOutput.AppliedSpecialCouponIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        List<(List<ItemForBasketOutputDto> BasketItems, SpecialCoupon PossibleSpecialCoupon, Money PossibleDiscountAmount)> discountElements = [];
        foreach (var possibleSpecialCoupon in possibleSpecialCoupons)
        {
            var basketItems = basketOutput.Items
                .Where(x => possibleSpecialCoupon.IsValid(x.CategoryIds, [(x.ProductId, x.VariantId)], basketTotalAmount))
                .ToList();

            if (basketItems.Count == 0)
            {
                continue;
            }

            var minPriceValue = basketItems.Select(x => x.Price.Value).Min();
            var totalQuantity = basketItems.Sum(x => x.Quantity);
            var possibleDiscountAmount = possibleSpecialCoupon.Discount.GetDiscountAmount(minPriceValue, totalQuantity);

            if (possibleDiscountAmount == Money.Zero && !possibleSpecialCoupon.Discount.IsFreeShippingDiscount)
            {
                continue;
            }

            discountElements.Add((basketItems, possibleSpecialCoupon, possibleDiscountAmount));
        }

        foreach (var discountElement in discountElements.OrderByDescending(x => x.PossibleDiscountAmount.Value))
        {
            if (discountElement.PossibleSpecialCoupon.Discount.IsFreeShippingDiscount)
            {
                basketOutput.HasFreeShippingCoupon = true;
                continue;
            }

            var totalValueWithDiscount = discountElement.BasketItems.Sum(x => x.TotalWithDiscount.Value);
            if (totalValueWithDiscount == 0)
            {
                continue;
            }

            var couponForBasket = new CouponForBasketOutputDto()
            {
                Id = discountElement.PossibleSpecialCoupon.Id,
                Description = $"({discountElement.PossibleSpecialCoupon.Code}) - {discountElement.PossibleSpecialCoupon.Description}",
                DiscountAmount = discountElement.PossibleDiscountAmount.ToMoneyOutputDto(),
                IsApplied = true,
                IsSpecialCoupon = true,
                Code = discountElement.PossibleSpecialCoupon.Code
            };
            basketOutput.AllPossibleCoupons.Add(couponForBasket);

            var discountAmountValuePerBasketItem = discountElement.PossibleDiscountAmount.Value / discountElement.BasketItems.Count;

            foreach (var basketItem in discountElement.BasketItems)
            {
                var totalValueWithDiscountPerBasketItem = basketItem.TotalWithDiscount.Value - discountAmountValuePerBasketItem;
                if (totalValueWithDiscountPerBasketItem < 0)
                {
                    totalValueWithDiscountPerBasketItem = 0;
                }
                basketItem.TotalWithDiscount = new Money(totalValueWithDiscountPerBasketItem).ToMoneyOutputDto();
            }
        }
    }

    public async Task ApplyCouponsAsync(BasketOutputDto basketOutput, CancellationToken cancellationToken = default)
    {
        await ApplyRegularCouponsAsync(basketOutput, cancellationToken);
        await ApplySpecialCouponsAsync(basketOutput, cancellationToken);
    }

    public async Task<Guid> GetSpecialCouponIdByCodeAsync(string couponCode, CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        var specialCouponId = await _discountDbContext.SpecialCoupon
            .Where(x =>
                x.Code == couponCode &&
                x.UsageCount.Value < x.UsageCount.Limit &&
                utcNow >= x.DateRange.UtcStartDateTime && utcNow <= x.DateRange.UtcEndDateTime)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (specialCouponId == Guid.Empty)
        {
            throw new ArgumentException("Kupon kodu geçersiz!", nameof(couponCode));
        }

        return specialCouponId;
    }
}
