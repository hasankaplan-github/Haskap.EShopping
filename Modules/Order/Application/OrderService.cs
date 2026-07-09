using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Application.Contracts.Account;
using Modules.Basket.Application.Contracts;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Application.Dtos;
using Modules.Discount.Application.Contracts;
using Modules.Order.Application.Contracts;
using Modules.Order.Application.Dtos;
using Modules.Order.Domain;
using Modules.Shipping.Application.Contracts;
using Modules.Shipping.Application.Dtos;
using System.Transactions;

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
        var existingOrder = await _orderDbContext.Order
            .Where(x => x.IdempotencyKey == input.IdempotencyKey)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingOrder is not null)
        {
            return existingOrder.Code.Value;
        }


        var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);


        if (_currentUserIdProvider.CurrentUserId is not null)
        {
            var currentAccount = await _accountService.GetByIdAsync(_currentUserIdProvider.CurrentUserId.Value, cancellationToken);

            input.FirstName = currentAccount.FirstName;
            input.LastName = currentAccount.LastName;
            input.PhoneNumber = currentAccount.PhoneNumber;
            input.EmailAddress = currentAccount.EmailAddress;
        }

        var orderAccount = new Order.Domain.OrderAggregate.Account(
            _currentUserIdProvider.CurrentUserId,
            input.FirstName,
            input.LastName,
            input.PhoneNumber,
            input.EmailAddress);

        var shippingAddressOutput = (_currentUserIdProvider.CurrentUserId is not null
            ? await _shippingService.GetAccountShippingAddressAsync(cancellationToken)
            : await _shippingService.GetAnonymousShippingAddressAsync(cancellationToken))!;

        var orderShippingAddress = new Order.Domain.OrderAggregate.ShippingAddress(
            shippingAddressOutput.Id,
            shippingAddressOutput.CityName,
            shippingAddressOutput.DistrictName,
            shippingAddressOutput.NeighborhoodName,
            shippingAddressOutput.Street,
            shippingAddressOutput.Postcode,
            shippingAddressOutput.BuildingNo,
            shippingAddressOutput.Floor,
            shippingAddressOutput.ApartmentNo,
            shippingAddressOutput.AddressLine);

        var basket = await _basketService.GetBasketAsync(cancellationToken);
        if (basket.Items.Count == 0)
        {
            throw new InvalidOperationException("Sepetinizde ürün bulunmuyor!");
        }
        await _discountService.ApplyCouponsAsync(basket);

        var shippingAddress = new ShippingAddressInputDto
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

        await _shippingService.ApplyShippingFeeAsync(basket, shippingAddress, cancellationToken);

        var orderShippingFee = new Money(basket.ShippingFee.Value);
        var orderTotal = new Money(basket.Total.Value);
        var orderTotalWithDiscount = new Money(basket.TotalWithDiscount.Value);

        var order = new Order.Domain.OrderAggregate.Order(
            GuidGenerator.CreateSimpleGuid(),
            input.IdempotencyKey,
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

        return order.Code.Value;
    }
}
