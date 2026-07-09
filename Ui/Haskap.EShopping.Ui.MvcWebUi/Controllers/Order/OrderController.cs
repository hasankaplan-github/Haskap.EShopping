using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Basket.Application.Contracts;
using Modules.Order.Application.Contracts;
using Modules.Order.Application.Dtos;
using Modules.Shipping.Application.Contracts;

namespace Haskap.EShopping.Ui.MvcWebUi.Controllers.Order;

public class OrderController : Controller
{
    private static readonly SemaphoreSlim s_orderSemaphore = new SemaphoreSlim(1, 1);

    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IShippingService _shippingService;
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public OrderController(
        ICurrentUserIdProvider currentUserIdProvider,
        IOrderService orderService,
        IBasketService basketService,
        IShippingService shippingService)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _orderService = orderService;
        _basketService = basketService;
        _shippingService = shippingService;
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var itemCount = await _basketService.GetItemCountAsync(cancellationToken);
        if (itemCount == 0)
        {
            throw new InvalidOperationException("Sepetinizde ürün bulunmuyor!");
        }

        var shippingAddressOutput = _currentUserIdProvider.CurrentUserId is not null
            ? await _shippingService.GetAccountShippingAddressAsync(cancellationToken)
            : await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken);

        if (shippingAddressOutput is null)
        {
            throw new InvalidOperationException("Geçerli bir gönderim adresiniz bulunmuyor!");
        }

        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Create(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        await s_orderSemaphore.WaitAsync(cancellationToken);
        try
        {
            var orderCode = await _orderService.CreateOrderAsync(input, cancellationToken);
            return Json(new { OrderCode = orderCode });
        }
        finally
        {
            s_orderSemaphore.Release();
        }
    }
}
