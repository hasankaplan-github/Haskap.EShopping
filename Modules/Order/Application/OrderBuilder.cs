using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Application.Contracts.Account;
using Modules.Basket.Application.Contracts;
using Modules.Basket.Application.Dtos;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Application.Dtos;
using Modules.Discount.Application.Contracts;
using Modules.Order.Application.Dtos;
using Modules.Order.Domain;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;
using System.Transactions;

namespace Modules.Order.Application;

internal class OrderBuilder
{
    private readonly IOrderDbContext _orderDbContext;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IAccountService _accountService;
    private readonly IShippingService _shippingService;
    private readonly IBasketService _basketService;
    private readonly IDiscountService _discountService;
    private readonly ICatalogService _catalogService;

    private Guid _idempotencyKey;

    private Func<CancellationToken, Task<Order.Domain.OrderAggregate.Account>> _withAccountAsyncFunc;
    private Func<CancellationToken, Task<(Order.Domain.OrderAggregate.ShippingAddress, ShippingAddressInputDto)>> _withShippingAddressAsyncFunc;
    private Func<CancellationToken, Task<BasketOutputDto>> _withBasketAsyncFunc;


    public OrderBuilder(
        IOrderDbContext orderDbContext,
        ICurrentUserIdProvider currentUserIdProvider,
        IAccountService accountService,
        IShippingService shippingService,
        IBasketService basketService,
        IDiscountService discountService,
        ICatalogService catalogService)
    {
        _orderDbContext = orderDbContext;
        _currentUserIdProvider = currentUserIdProvider;
        _accountService = accountService;
        _shippingService = shippingService;
        _basketService = basketService;
        _discountService = discountService;
        _catalogService = catalogService;
    }

    public OrderBuilder WithIdempotencyKey(Guid idempotencyKey)
    {
        _idempotencyKey = idempotencyKey;
        return this;
    }

    public OrderBuilder WithAccount(CreateInputDto input)
    {
        _withAccountAsyncFunc = async (cancellationToken) =>
        {
            if (_currentUserIdProvider.CurrentUserId is not null)
            {
                var currentAccount = await _accountService.GetByIdAsync(_currentUserIdProvider.CurrentUserId.Value, cancellationToken);
                input.FirstName = currentAccount.FirstName;
                input.LastName = currentAccount.LastName;
                input.PhoneNumber = currentAccount.PhoneNumber;
                input.EmailAddress = currentAccount.EmailAddress;
            }

            return new Order.Domain.OrderAggregate.Account(
                _currentUserIdProvider.CurrentUserId,
                input.FirstName,
                input.LastName,
                input.PhoneNumber,
                input.EmailAddress);
        };

        return this;
    }

    public OrderBuilder WithShippingAddress()
    {
        _withShippingAddressAsyncFunc = async (cancellationToken) =>
        {
            var shippingAddressOutput = (_currentUserIdProvider.CurrentUserId is not null
            ? await _shippingService.GetAccountShippingAddressAsync(cancellationToken)
            : await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken))!;

            var shippingAddressInputDto = new ShippingAddressInputDto
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

            return (new Order.Domain.OrderAggregate.ShippingAddress(
                shippingAddressOutput.Id,
                shippingAddressOutput.CityName,
                shippingAddressOutput.DistrictName,
                shippingAddressOutput.NeighborhoodName,
                shippingAddressOutput.Street,
                shippingAddressOutput.Postcode,
                shippingAddressOutput.BuildingNo,
                shippingAddressOutput.Floor,
                shippingAddressOutput.ApartmentNo,
                shippingAddressOutput.AddressLine), shippingAddressInputDto);
        };
        
        return this;
    }

    public OrderBuilder WithBasket()
    {
        _withBasketAsyncFunc = async (cancellationToken) =>
        {
            var basket = await _basketService.GetBasketAsync(cancellationToken);

            if (basket.Items.Count == 0)
            {
                throw new InvalidOperationException("Sepetinizde ürün bulunmuyor!");
            }

            return basket;
        };

        return this;
    }

    public async Task<Order.Domain.OrderAggregate.Order> BuildAsync(CancellationToken cancellationToken = default)
    {
        var existingOrder = await _orderDbContext.Order
            .Where(x => x.IdempotencyKey == _idempotencyKey)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingOrder is not null)
        {
            return existingOrder;
        }

        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

        var orderAccount = await _withAccountAsyncFunc(cancellationToken);
        var (orderShippingAddress, shippingAddressInputDto) = await _withShippingAddressAsyncFunc(cancellationToken);
        var basket = await _withBasketAsyncFunc(cancellationToken);

        await _discountService.ApplyCouponsAsync(basket, cancellationToken);
        await _shippingService.ApplyShippingFeeAsync(basket, shippingAddressInputDto, cancellationToken);

        var orderShippingFee = new Money(basket.ShippingFee.Value);
        var orderTotal = new Money(basket.Total.Value);
        var orderTotalWithDiscount = new Money(basket.TotalWithDiscount.Value);

        var order = new Order.Domain.OrderAggregate.Order(
            GuidGenerator.CreateSimpleGuid(),
            _idempotencyKey,
            orderAccount,
            orderShippingAddress,
            basket.HasFreeShippingCoupon,
            orderShippingFee,
            orderTotal,
            orderTotalWithDiscount);

        var sellInput = new SellInputDto();
        foreach (var basketItem in basket.Items)
        {
            var orderItem = new Order.Domain.OrderAggregate.Item(
                GuidGenerator.CreateSimpleGuid(),
                basketItem.ProductId,
                basketItem.VariantId,
                basketItem.ProductName,
                basketItem.VariantSkuValue,
                basketItem.SlugValue,
                new Money(basketItem.Price.Value),
                basketItem.Quantity,
                new Money(basketItem.Total.Value),
                new Money(basketItem.TotalWithDiscount.Value));

            order.AddItem(orderItem);

            sellInput.Items.Add((basketItem.ProductId, basketItem.VariantId, basketItem.Quantity));
        }

        await _catalogService.SellAsync(sellInput, cancellationToken);



        foreach (var coupon in basket.AllPossibleCoupons.Where(x => x.IsApplied))
        {
            var orderAppliedCoupon = new Order.Domain.OrderAggregate.AppliedCoupon(
                GuidGenerator.CreateSimpleGuid(),
                coupon.Id,
                coupon.Description,
                new Money(coupon.DiscountAmount.Value),
                coupon.IsSpecialCoupon,
                coupon.Code);

            order.AddAppliedCoupon(orderAppliedCoupon);
        }

        _orderDbContext.Order.Add(order);
        await _orderDbContext.SaveChangesAsync(cancellationToken);

        await _basketService.RemoveBasketAsync(cancellationToken);


        transactionScope.Complete();

        return order;
    }
}