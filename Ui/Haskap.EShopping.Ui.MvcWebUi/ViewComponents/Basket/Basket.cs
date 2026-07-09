using Microsoft.AspNetCore.Mvc;
using Modules.Basket.Application.Contracts;
using Modules.Discount.Application.Contracts;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Basket;

public class Basket : ViewComponent
{
    private readonly IBasketService _basketService;
    private readonly IDiscountService _discountService;
    private readonly IShippingService _shippingService;

    public Basket(
        IBasketService basketService,
        IDiscountService discountService,
        IShippingService shippingService)
    {
        _basketService = basketService;
        _discountService = discountService;
        _shippingService = shippingService;
    }

    public async Task<IViewComponentResult> InvokeAsync(ShippingAddressInputDto? shippingAddress, CancellationToken cancellationToken = default)
    {
        var basket = await _basketService.GetBasketAsync(cancellationToken);
        await _discountService.ApplyCouponsAsync(basket);

        ViewBag.ShippingAddress = shippingAddress;

        return View(basket);
    }
}