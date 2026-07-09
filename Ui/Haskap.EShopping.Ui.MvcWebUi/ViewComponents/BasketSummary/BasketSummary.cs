using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Basket.Application.Contracts;
using Modules.Discount.Application.Contracts;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.BasketSummary;

public class BasketSummary : ViewComponent
{
    private readonly IBasketService _basketService;
    private readonly IDiscountService _discountService;
    private readonly IShippingService _shippingService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public BasketSummary(
        IBasketService basketService,
        IDiscountService discountService,
        IShippingService shippingService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _basketService = basketService;
        _discountService = discountService;
        _shippingService = shippingService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var basket = await _basketService.GetBasketAsync(cancellationToken);
        await _discountService.ApplyCouponsAsync(basket);

        var shippingAddressOutput = (_currentUserIdProvider.CurrentUserId is not null
            ? await _shippingService.GetAccountShippingAddressAsync(cancellationToken)
            : await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken))!;

        var shippingAddress = new ShippingAddressInputDto
        {
            CityId = shippingAddressOutput.CityId,
            DistrictId = shippingAddressOutput.DistrictId,
            NeighborhoodId = shippingAddressOutput.NeighborhoodId,
            Street = shippingAddressOutput.Street,
            Postcode = shippingAddressOutput.Postcode,
            BuildingNo = shippingAddressOutput.BuildingNo,
            Floor = shippingAddressOutput.Floor,
            ApartmentNo = shippingAddressOutput.ApartmentNo,
            AddressLine = shippingAddressOutput.AddressLine
        };

        await _shippingService.ApplyShippingFeeAsync(basket, shippingAddress, cancellationToken);

        return View(basket);
    }
}