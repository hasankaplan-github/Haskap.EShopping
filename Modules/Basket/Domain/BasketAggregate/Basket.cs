using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using Modules.Basket.Domain.Shared.Consts;
using Modules.Catalog.Application.Contracts;

namespace Modules.Basket.Domain.BasketAggregate;

public class Basket : AggregateRoot, IAuditable
{
    private static readonly Lock s_itemLock = new();

    public Guid OwnerAccountId { get; private set; }
    public IReadOnlyList<BasketItem> Items => _items.AsReadOnly();
    private List<BasketItem> _items = [];
    public IReadOnlyList<BasketAppliedSpecialCoupon> AppliedSpecialCoupons => _appliedSpecialCoupons.AsReadOnly();
    private List<BasketAppliedSpecialCoupon> _appliedSpecialCoupons = [];
    public IReadOnlyList<BasketRemovedRegularCoupon> RemovedRegularCoupons => _removedRegularCoupons.AsReadOnly();
    private List<BasketRemovedRegularCoupon> _removedRegularCoupons = [];
    public Guid? CreatedUserId { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    private Basket()
    {}

    public Basket(Guid id, Guid ownerAccountId)
        : base(id)
    {
        OwnerAccountId = ownerAccountId;
    }

    public void Touch()
    {
        ModifiedOnUtc = DateTime.UtcNow;
    }

    public void AddJustOneItem(Guid productId, Guid productVariantId, IStockCheckerService stockCheckerService)
    {
        lock (s_itemLock)
        {
            var existingItem = _items.Where(x => x.ProductId == productId && x.ProductVariantId == productVariantId).FirstOrDefault();
            if (existingItem is not null)
            {
                throw new InvalidOperationException("Ürün zaten sepete eklenmiş durumda!");
            }

            if (!stockCheckerService.IsStockAvailableAsync(productId, productVariantId, 1).GetAwaiter().GetResult())
            {
                throw new InvalidOperationException("Ürün stokta bulunmamaktadır!");
            }

            _items.Add(new BasketItem(GuidGenerator.CreateSimpleGuid(), productId, productVariantId, 1));
        }

        Touch();
    }

    public void AddOrUpdateItem(Guid productId, Guid productVariantId, int newQuantity, IStockCheckerService stockCheckerService)
    {
        lock (s_itemLock)
        {
            if (!stockCheckerService.IsStockAvailableAsync(productId, productVariantId, newQuantity).GetAwaiter().GetResult())
            {
                throw new InvalidOperationException("İstediğiniz miktarda ürün stokta bulunmamaktadır!");
            }

            var existingItem = _items.Where(x => x.ProductId == productId && x.ProductVariantId == productVariantId).FirstOrDefault();
            if (existingItem is not null)
            {
                existingItem.SetQuantity(newQuantity);
            }
            else
            {
                _items.Add(new BasketItem(GuidGenerator.CreateSimpleGuid(), productId, productVariantId, newQuantity));
            }
        }

        Touch();
    }

    public void RemoveItem(Guid productId, Guid productVariantId)
    {
        lock (s_itemLock)
        {
            _items.RemoveAll(x => x.ProductId == productId && x.ProductVariantId == productVariantId);
        }

        Touch();
    }

    internal bool IsExpired()
    {
        var utcLastUpdatedOn = ModifiedOnUtc ?? CreatedOnUtc;
        if (utcLastUpdatedOn is null) return false;

        var utcExpirationDate = utcLastUpdatedOn.Value.AddDays(BasketConsts.LifetimeInDays);
        return DateTime.UtcNow > utcExpirationDate;
    }

    internal void MergeWith(Basket? otherBasket, IStockCheckerService stockCheckerService)
    {
        if (otherBasket is null) return;

        lock (s_itemLock)
        {
            foreach (var otherItem in otherBasket.Items)
            {
                var existingItem = _items.Where(x => x.ProductId == otherItem.ProductId && x.ProductVariantId == otherItem.ProductVariantId).FirstOrDefault();
                if (existingItem is not null)
                {
                    if (!stockCheckerService.IsStockAvailableAsync(otherItem.ProductId, otherItem.ProductVariantId, existingItem.Quantity + otherItem.Quantity).GetAwaiter().GetResult())
                    {
                        continue;
                    }

                    existingItem.SetQuantity(existingItem.Quantity + otherItem.Quantity);
                }
                else
                {
                    if (!stockCheckerService.IsStockAvailableAsync(otherItem.ProductId, otherItem.ProductVariantId, otherItem.Quantity).GetAwaiter().GetResult())
                    {
                        continue;
                    }

                    _items.Add(new BasketItem(GuidGenerator.CreateSimpleGuid(), otherItem.ProductId, otherItem.ProductVariantId, otherItem.Quantity));
                }
            }
        }

        foreach (var otherAppliedSpecialCouponId in otherBasket.AppliedSpecialCoupons.Select(x => x.SpecialCouponId))
        {
            ApplySpecialCoupon(otherAppliedSpecialCouponId);
        }

        foreach (var otherRemovedRegularCouponId in otherBasket.RemovedRegularCoupons.Select(x => x.RegularCouponId))
        {
            RemoveRegularCoupon(otherRemovedRegularCouponId);
        }

        Touch();
    }

    public void ApplySpecialCoupon(Guid specialCouponId)
    {
        if (_appliedSpecialCoupons.Any(x => x.SpecialCouponId == specialCouponId)) return;

        _appliedSpecialCoupons.Add(new BasketAppliedSpecialCoupon(GuidGenerator.CreateSimpleGuid())
        {
            SpecialCouponId = specialCouponId
        });

        Touch();
    }

    public void RemoveSpecialCoupon(Guid specialCouponId)
    {
        _appliedSpecialCoupons.RemoveAll(x => x.SpecialCouponId == specialCouponId);

        Touch();
    }

    public void RemoveRegularCoupon(Guid regularCouponId)
    {
        if (_removedRegularCoupons.Any(x => x.RegularCouponId == regularCouponId)) return;

        _removedRegularCoupons.Add(new BasketRemovedRegularCoupon(GuidGenerator.CreateSimpleGuid())
        {
            RegularCouponId = regularCouponId
        });

        Touch();
    }

    public void ApplyRegularCoupon(Guid regularCouponId)
    {
        _removedRegularCoupons.RemoveAll(x => x.RegularCouponId == regularCouponId);

        Touch();
    }
}
