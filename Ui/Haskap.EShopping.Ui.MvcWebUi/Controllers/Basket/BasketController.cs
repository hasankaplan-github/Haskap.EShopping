using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Modules.Basket.Application.Contracts;
using Modules.Basket.Application.Dtos;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;
using System.Text.Encodings.Web;

namespace Haskap.EShopping.Ui.MvcWebUi.Controllers.Basket;

public class BasketController : Controller
{
    private readonly IBasketService _basketService;
    private readonly IShippingService _shippingService;
    private readonly IViewComponentHelper _viewComponentHelper;

    public BasketController(
        IBasketService basketService,
        IShippingService shippingService,
        IViewComponentHelper viewComponentHelper)
    {
        _basketService = basketService;
        _shippingService = shippingService;
        _viewComponentHelper = viewComponentHelper;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        return View();
    }

    [HttpGet]
    public async Task<int> GetItemCount(CancellationToken cancellationToken = default)
    {
        return await _basketService.GetItemCountAsync(cancellationToken);
    }

    [HttpPost]
    public async Task AddOrUpdateItem(AddOrUpdateItemInputDto input, CancellationToken cancellationToken = default)
    {
        await _basketService.AddOrUpdateItemAsync(input, cancellationToken);
    }

    [HttpPost]
    public async Task RemoveItemFromBasket(RemoveItemFromBasketInputDto input, CancellationToken cancellationToken = default)
    {
        await _basketService.RemoveItemFromBasketAsync(input, cancellationToken);
    }

    //[HttpPost]
    //public async Task<IActionResult> LoadBasketViewComponent(ShippingAddressInputDto shippingAddressInput, CancellationToken cancellationToken = default)
    //{
    //    return ViewComponent(typeof(ViewComponents.Basket.Basket), new { ShippingAddress = shippingAddressInput });
    //}

    [HttpPost]
    public async Task<JsonResult> LoadBasketViewComponent(ShippingAddressInputDto shippingAddressInput, CancellationToken cancellationToken = default)
    {
        var componentHtml = string.Empty;
        using (var writer = new StringWriter())
        {
            var viewContext = new ViewContext(ControllerContext, new MyNullView(), ViewData, TempData, writer, new HtmlHelperOptions());
            (_viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

            var result = await _viewComponentHelper.InvokeAsync<ViewComponents.Basket.Basket>(new { shippingAddress = shippingAddressInput });
            result.WriteTo(writer, HtmlEncoder.Default);
            writer.Flush();
            componentHtml = writer.ToString();
        }

        return Json(new { Html = componentHtml });
    }

    [HttpPost]
    public async Task ApplySpecialCoupon(string couponCode, CancellationToken cancellationToken = default)
    {
        await _basketService.ApplySpecialCouponAsync(couponCode, cancellationToken);
    }

    [HttpPost]
    public async Task RemoveSpecialCoupon(Guid specialCouponId, CancellationToken cancellationToken = default)
    {
        await _basketService.RemoveSpecialCouponAsync(specialCouponId, cancellationToken);
    }

    [HttpPost]
    public async Task ApplyRegularCoupon(Guid regularCouponId, CancellationToken cancellationToken = default)
    {
        await _basketService.ApplyRegularCouponAsync(regularCouponId, cancellationToken);
    }

    [HttpPost]
    public async Task RemoveRegularCoupon(Guid regularCouponId, CancellationToken cancellationToken = default)
    {
        await _basketService.RemoveRegularCouponAsync(regularCouponId, cancellationToken);
    }

    [HttpGet]
    public async Task<JsonResult> GetDistrictsByCityId(Guid? cityId, CancellationToken cancellationToken = default)
    {
        var districts = await _shippingService.GetDistrictsByCityIdAsync(cityId, cancellationToken);
        return Json(districts);
    }

    [HttpGet]
    public async Task<JsonResult> GetNeighborhoodsByDistrictId(Guid? districtId, CancellationToken cancellationToken = default)
    {
        var neighborhoods = await _shippingService.GetNeighborhoodsByDistrictIdAsync(districtId, cancellationToken);
        return Json(neighborhoods);
    }

    [HttpPost]
    public async Task ProceedToCreatingOrder(ShippingAddressInputDto input, CancellationToken cancellationToken = default)
    {
        await _basketService.ProceedToCreatingOrderAsync(input, cancellationToken);
    }
}
