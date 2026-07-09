using Haskap.DddBase.Application.Contracts;
using Modules.Basket.Application.Dtos;
using Modules.Shipping.Application.Dtos;

namespace Modules.Basket.Application.Contracts;

public interface IBasketService : IUseCaseService
{
    Task AddJustOneItemAsync(AddJustOneItemInputDto input, CancellationToken cancellationToken = default);
    Task AddOrUpdateItemAsync(AddOrUpdateItemInputDto input, CancellationToken cancellationToken = default);
    Task RemoveItemFromBasketAsync(RemoveItemFromBasketInputDto input, CancellationToken cancellationToken = default);
    Task<BasketOutputDto> GetBasketAsync(CancellationToken cancellationToken = default);
    Task<int> GetItemCountAsync(CancellationToken cancellationToken = default);
    Task ApplySpecialCouponAsync(string couponCode, CancellationToken cancellationToken = default);
    Task RemoveSpecialCouponAsync(Guid specialCouponId, CancellationToken cancellationToken = default);
    Task ApplyRegularCouponAsync(Guid regularCouponId, CancellationToken cancellationToken = default);
    Task RemoveRegularCouponAsync(Guid regularCouponId, CancellationToken cancellationToken = default);
    Task ProceedToCreatingOrderAsync(ShippingAddressInputDto input, CancellationToken cancellationToken = default);
    Task RemoveBasketAsync(CancellationToken cancellationToken = default);
}
