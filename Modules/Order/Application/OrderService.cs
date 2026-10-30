using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Providers;
using Modules.Account.Application.Contracts.Account;
using Modules.Basket.Application.Contracts;
using Modules.Catalog.Application.Contracts;
using Modules.Discount.Application.Contracts;
using Modules.Order.Application.Contracts;
using Modules.Order.Application.Dtos;
using Modules.Order.Domain;
using Modules.Shipping.Application.Contracts;

namespace Modules.Order.Application;

public class OrderService : UseCaseService, IOrderService
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IAccountService _accountService;
    private readonly IShippingService _shippingService;
    private readonly IBasketService _basketService;
    private readonly IDiscountService _discountService;
    private readonly ICatalogService _catalogService;
    private readonly IOrderDbContext _orderDbContext;

    public OrderService(
        IAccountService accountService,
        ICurrentUserIdProvider currentUserIdProvider,
        IShippingService shippingService,
        IBasketService basketService,
        IDiscountService discountService,
        ICatalogService catalogService,
        IOrderDbContext orderDbContext)
    {
        _accountService = accountService;
        _currentUserIdProvider = currentUserIdProvider;
        _shippingService = shippingService;
        _basketService = basketService;
        _discountService = discountService;
        _catalogService = catalogService;
        _orderDbContext = orderDbContext;
    }

    public async Task<string> CreateOrderAsync(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var order = await new OrderBuilder(
            _orderDbContext,
            _currentUserIdProvider,
            _accountService,
            _shippingService,
            _basketService,
            _discountService,
            _catalogService)
            .WithIdempotencyKey(input.IdempotencyKey)
            .WithAccount(input)
            .WithShippingAddress()
            .WithBasket()
            .BuildAsync(cancellationToken);

        return order.Code.Value;
    }
}
