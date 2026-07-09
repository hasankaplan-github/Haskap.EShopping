using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Shipping.Application.Contracts;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.ShippingAddressSummary;

public class ShippingAddressSummary : ViewComponent
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IShippingService _shippingService;

    public ShippingAddressSummary(
        IShippingService shippingService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _shippingService = shippingService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var shippingAddressOutput = _currentUserIdProvider.CurrentUserId is not null
            ? await _shippingService.GetAccountShippingAddressAsync(cancellationToken)
            : await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken);

        return View(shippingAddressOutput!);
    }
}