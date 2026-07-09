using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Basket.Application.Dtos;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Shipping;

public class Shipping : ViewComponent
{
    private readonly IShippingService _shippingService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public Shipping(
        IShippingService shippingService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _shippingService = shippingService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(ShippingAddressInputDto? shippingAddress, BasketOutputDto basket, CancellationToken cancellationToken = default)
    {
        ViewBag.Basket = basket;

        if (_currentUserIdProvider.CurrentUserId is not null)
        {
            var accountShippingAddress = (await _shippingService.GetAccountShippingAddressAsync(cancellationToken))!;

            shippingAddress = new ShippingAddressInputDto
            {
                CityId = accountShippingAddress.CityId,
                DistrictId = accountShippingAddress.DistrictId,
                NeighborhoodId = accountShippingAddress.NeighborhoodId,
                Street = accountShippingAddress.Street,
                Postcode = accountShippingAddress.Postcode,
                BuildingNo = accountShippingAddress.BuildingNo,
                Floor = accountShippingAddress.Floor,
                ApartmentNo = accountShippingAddress.ApartmentNo,
                AddressLine = accountShippingAddress.AddressLine
            };

            await _shippingService.ApplyShippingFeeAsync(basket, shippingAddress, cancellationToken);

            return View("AccountShippingAddress", accountShippingAddress!);
        }

        if (shippingAddress is null)
        {
            var anonymousShippingAddress = await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken);

            // If the user is not logged in and there is no shipping address provided, use the anonymous shipping address if it exists
            // Sayfa ilk kez açılıyordur, provided adres null'dur ve bu basket için adres daha önce kaydedilmiş olabilir. Bu durumda anonymousShippingAddress kullanılır.
            if (anonymousShippingAddress is not null)
            {
                shippingAddress = new ShippingAddressInputDto
                {
                    CityId = anonymousShippingAddress.CityId,
                    DistrictId = anonymousShippingAddress.DistrictId,
                    NeighborhoodId = anonymousShippingAddress.NeighborhoodId,
                    Street = anonymousShippingAddress.Street,
                    Postcode = anonymousShippingAddress.Postcode,
                    BuildingNo = anonymousShippingAddress.BuildingNo,
                    Floor = anonymousShippingAddress.Floor,
                    ApartmentNo = anonymousShippingAddress.ApartmentNo,
                    AddressLine = anonymousShippingAddress.AddressLine
                };
            }
        }

        await _shippingService.ApplyShippingFeeAsync(basket, shippingAddress, cancellationToken);

        ViewBag.Cities = await _shippingService.GetAllCitiesAsync(cancellationToken);
        ViewBag.Districts = await _shippingService.GetDistrictsByCityIdAsync(shippingAddress?.CityId, cancellationToken);
        ViewBag.Neighborhoods = await _shippingService.GetNeighborhoodsByDistrictIdAsync(shippingAddress?.DistrictId, cancellationToken);

        return View(shippingAddress);
    }
}
