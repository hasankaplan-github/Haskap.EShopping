using Haskap.DddBase.Application.Contracts;
using Haskap.EShopping.Application.Dtos.Common;
using Modules.Basket.Application.Dtos;
using Modules.Shipping.Application.Dtos;

namespace Modules.Shipping.Application.Contracts;

public interface IShippingService : IUseCaseService
{
    Task ApplyShippingFeeAsync(BasketOutputDto basketOutput, ShippingAddressInputDto? shippingAddress, CancellationToken cancellationToken = default);
    Task<MoneyOutputDto> GetShippingFeeAsync(ShippingAddressInputDto? shippingAddressInput, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CityOutputDto>> GetAllCitiesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DistrictOutputDto>> GetDistrictsByCityIdAsync(Guid? cityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NeighborhoodOutputDto>> GetNeighborhoodsByDistrictIdAsync(Guid? districtId, CancellationToken cancellationToken = default);
    Task<ShippingAddressOutputDto?> GetAccountShippingAddressAsync(CancellationToken cancellationToken);
    Task<ShippingAddressOutputDto?> GetAnonymousShippingAddressAsync(CancellationToken cancellationToken);
    Task SaveOrUpdateAnonymousShippingAddressAsync(ShippingAddressInputDto shippingAddressInput, CancellationToken cancellationToken = default);
}
