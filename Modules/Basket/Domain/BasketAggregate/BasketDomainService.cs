using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Services;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Contracts;
using Modules.CustomMessage.Application.Contracts;

namespace Modules.Basket.Domain.BasketAggregate;

public class BasketDomainService : DomainService
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IBasketDbContext _basketDbContext;
    private readonly IAnonymousAccountProvider _anonymousAccountProvider;
    private readonly IStockCheckerService _stockCheckerService;
    private readonly ICustomMessageService _customMessageService;

    public BasketDomainService(
        ICurrentUserIdProvider currentUserIdProvider,
        IBasketDbContext basketDbContext,
        IAnonymousAccountProvider anonymousAccountProvider,
        IStockCheckerService stockCheckerService,
        ICustomMessageService customMessageService)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _basketDbContext = basketDbContext;
        _anonymousAccountProvider = anonymousAccountProvider;
        _stockCheckerService = stockCheckerService;
        _customMessageService = customMessageService;
    }

    private async Task<Basket?> GetOrCreateAnonymousBasketAsync(CancellationToken cancellationToken = default)
    {
        var existingBasket = await _basketDbContext.Basket
            .Include(b => b.Items)
            .Include(x => x.AppliedSpecialCoupons)
            .Include(x => x.RemovedRegularCoupons)
            .Where(b => b.OwnerAccountId == _anonymousAccountProvider.AccountId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (existingBasket is not null && !existingBasket.IsExpired())
        {
            return existingBasket;
        }

        if (existingBasket is not null)
        {
            // Remove the expired basket
            _basketDbContext.Basket.Remove(existingBasket);
            await _basketDbContext.SaveChangesAsync(cancellationToken);
        }

        if(_currentUserIdProvider.CurrentUserId is null)
        {
            // Not logged in - Create a new anonymous basket
            var newBasket = new Basket(GuidGenerator.CreateSimpleGuid(), _anonymousAccountProvider.AccountId);
            _basketDbContext.Basket.Add(newBasket);
            await _basketDbContext.SaveChangesAsync(cancellationToken);

            return newBasket;
        }

        // Logged in - No need for an anonymous basket
        return null;
    }

    private async Task<Basket?> GetOrCreateOwnerAccountsBasketAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserIdProvider.CurrentUserId is null)
        {
            return null;
        }

        var existingBasket = await _basketDbContext.Basket
            .Include(b => b.Items)
            .Include(x => x.AppliedSpecialCoupons)
            .Include(x => x.RemovedRegularCoupons)
            .Where(b => b.OwnerAccountId == _currentUserIdProvider.CurrentUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingBasket is not null && !existingBasket.IsExpired())
        {
            return existingBasket;
        }

        if (existingBasket is not null)
        {
            // Remove the expired basket
            _basketDbContext.Basket.Remove(existingBasket);
        }

        var newBasket = new Basket(GuidGenerator.CreateSimpleGuid(), _currentUserIdProvider.CurrentUserId.Value);
        _basketDbContext.Basket.Add(newBasket);
        await _basketDbContext.SaveChangesAsync(cancellationToken);

        return newBasket;
    }

    public async Task<Basket> GetValidBasketAsync(CancellationToken cancellationToken = default)
    {
        var anonymousBasket = await GetOrCreateAnonymousBasketAsync(cancellationToken);
        var ownerAccountsBasket = await GetOrCreateOwnerAccountsBasketAsync(cancellationToken);

        var totalItemQuantityBeforeMerge = (ownerAccountsBasket?.Items.Sum(i => i.Quantity) ?? 0) + (anonymousBasket?.Items.Sum(i => i.Quantity) ?? 0);

        var dbContextNeedsSave = false;

        if (ownerAccountsBasket is not null)
        {
            foreach (var basketItem in ownerAccountsBasket.Items.ToList())
            {
                if (!(await _stockCheckerService.IsStockAvailableAsync(basketItem.ProductId, basketItem.ProductVariantId, basketItem.Quantity)))
                {
                    ownerAccountsBasket!.RemoveItem(basketItem.ProductId, basketItem.ProductVariantId);
                    dbContextNeedsSave = true;
                }
            }

            ownerAccountsBasket.MergeWith(anonymousBasket, _stockCheckerService);
        }
        else if (anonymousBasket is not null)
        {
            foreach (var basketItem in anonymousBasket.Items.ToList())
            {
                if (!(await _stockCheckerService.IsStockAvailableAsync(basketItem.ProductId, basketItem.ProductVariantId, basketItem.Quantity)))
                {
                    anonymousBasket!.RemoveItem(basketItem.ProductId, basketItem.ProductVariantId);
                    dbContextNeedsSave = true;
                }
            }
        }

        if (ownerAccountsBasket is not null)
        {
            if (anonymousBasket is not null)
            {
                _basketDbContext.Basket.Remove(anonymousBasket);
                
                dbContextNeedsSave = true;
            }
        }

        if (dbContextNeedsSave)
        {
            await _basketDbContext.SaveChangesAsync(cancellationToken);
        }

        var validBasket = (ownerAccountsBasket ?? anonymousBasket)!;

        var totalItemQuantityAfterMerge = validBasket.Items.Sum(i => i.Quantity);

        if(totalItemQuantityAfterMerge != totalItemQuantityBeforeMerge)
        {
            _customMessageService.ShowWarningMessage("Uyarı!", "Bazı ürünler stok yetersizliğinden dolayı sepetinizden çıkarıldı. Lütfen sepetinizi kontrol ediniz.");
        }

        // Return the valid basket (owner's or anonymous)
        return validBasket;
    }
}
