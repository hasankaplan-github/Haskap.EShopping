using Haskap.DddBase.Application.Contracts;
using Modules.Basket.Application.Dtos;

namespace Modules.Discount.Application.Contracts;

public interface IDiscountService : IUseCaseService
{
    Task ApplyCouponsAsync(BasketOutputDto basketOutput, CancellationToken cancellationToken = default);
    Task<Guid> GetSpecialCouponIdByCodeAsync(string couponCode, CancellationToken cancellationToken = default);
}
