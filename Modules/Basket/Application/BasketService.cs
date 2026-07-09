using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Providers;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Modules.Basket.Application.Contracts;
using Modules.Basket.Application.Dtos;
using Modules.Basket.Domain;
using Modules.Basket.Domain.BasketAggregate;
using Modules.Catalog.Application.Contracts;
using Modules.Discount.Application.Contracts;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;

namespace Modules.Basket.Application;

internal class BasketService : UseCaseService, IBasketService
{
    private readonly BasketDomainService _basketDomainService;
    private readonly IBasketDbContext _basketDbContext;
    private readonly ICatalogService _catalogService;
    private readonly IDiscountService _discountService;
    private readonly IStockCheckerService _stockCheckerService;
    private readonly IShippingService _shippingService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public BasketService(
        BasketDomainService basketDomainService,
        IBasketDbContext basketDbContext,
        ICatalogService catalogService,
        IDiscountService discountService,
        IStockCheckerService stockCheckerService,
        IShippingService shippingService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _basketDomainService = basketDomainService;
        _basketDbContext = basketDbContext;
        _catalogService = catalogService;
        _discountService = discountService;
        _stockCheckerService = stockCheckerService;
        _shippingService = shippingService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task AddJustOneItemAsync(AddJustOneItemInputDto input, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);

        basket.AddJustOneItem(input.ProductId, input.ProductVariantId, _stockCheckerService);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddOrUpdateItemAsync(AddOrUpdateItemInputDto input, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);

        basket.AddOrUpdateItem(input.ProductId, input.ProductVariantId, input.Quantity, _stockCheckerService);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveItemFromBasketAsync(RemoveItemFromBasketInputDto input, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);

        basket.RemoveItem(input.ProductId, input.ProductVariantId);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<BasketOutputDto> GetBasketAsync(CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        var basketOutput = new BasketOutputDto() { Id = basket.Id };

        foreach(var basketItemGrouping in basket.Items.GroupBy(i => i.ProductId))
        {
            var basketOutputItems = await _catalogService.GetItemsForBasketAsync(basketItemGrouping.Key, basketItemGrouping.Select(i => i.ProductVariantId).ToList(), cancellationToken);

            foreach (var basketOutputItem in basketOutputItems)
            {
                var matchingBasketItem = basketItemGrouping.First(i => i.ProductVariantId == basketOutputItem.VariantId);
                basketOutputItem.Quantity = matchingBasketItem.Quantity;

                var itemTotal = new Money(basketOutputItem.Price.Value * matchingBasketItem.Quantity);
                basketOutputItem.Total = itemTotal.ToMoneyOutputDto();
                basketOutputItem.TotalWithDiscount = itemTotal.ToMoneyOutputDto();
            }

            basketOutput.Items.AddRange(basketOutputItems);
        }

        basketOutput.AppliedSpecialCouponIds = basket.AppliedSpecialCoupons.Select(x => x.SpecialCouponId).ToList();
        basketOutput.RemovedRegularCouponIds = basket.RemovedRegularCoupons.Select(x => x.RegularCouponId).ToList();

        return basketOutput;
    }

    public async Task<int> GetItemCountAsync(CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        return basket.Items.Sum(i => i.Quantity);
    }

    public async Task ApplySpecialCouponAsync(string couponCode, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        var specialCouponId = await _discountService.GetSpecialCouponIdByCodeAsync(couponCode, cancellationToken);
        
        basket.ApplySpecialCoupon(specialCouponId);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSpecialCouponAsync(Guid specialCouponId, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        basket.RemoveSpecialCoupon(specialCouponId);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ApplyRegularCouponAsync(Guid regularCouponId, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        basket.ApplyRegularCoupon(regularCouponId);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRegularCouponAsync(Guid regularCouponId, CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        basket.RemoveRegularCoupon(regularCouponId);

        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ProceedToCreatingOrderAsync(ShippingAddressInputDto input, CancellationToken cancellationToken = default)
    {
        if(_currentUserIdProvider.CurrentUserId is null)
        {
            await _shippingService.SaveOrUpdateAnonymousShippingAddressAsync(input, cancellationToken);
        }
    }

    public async Task RemoveBasketAsync(CancellationToken cancellationToken = default)
    {
        var basket = await _basketDomainService.GetValidBasketAsync(cancellationToken);
        _basketDbContext.Basket.Remove(basket);
        await _basketDbContext.SaveChangesAsync(cancellationToken);
    }
}
