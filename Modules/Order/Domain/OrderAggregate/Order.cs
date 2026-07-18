using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Order.Domain.OrderAggregate;

public class Order : AggregateRoot
{
    public Guid IdempotencyKey { get; private set; }
    public Code Code { get; private set; } = Code.Generate();
    public Account Account { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public IReadOnlyList<Item> Items => _items.AsReadOnly();
    private List<Item> _items = [];
    public IReadOnlyList<AppliedCoupon> AppliedCoupons => _appliedCoupons.AsReadOnly();
    private List<AppliedCoupon> _appliedCoupons = [];
    public bool HasFreeShippingCoupon { get; private set; }
    public Money ShippingFee { get; private set; }
    public Money Total { get; private set; }
    public Money TotalWithDiscount { get; private set; }

    private Order()
    {}

    public Order(
        Guid id,
        Guid idempotencyKey,
        Account account,
        ShippingAddress shippingAddress,
        bool hasFreeShippingCoupon,
        Money shippingFee,
        Money total,
        Money totalWithDiscount)
        : base(id)
    {
        if (idempotencyKey == Guid.Empty)
        {
            throw new ArgumentException("Idempotency Key geçersizdir, sayfayı yenileyerek tekrar deneyiniz.", nameof(idempotencyKey));
        }

        IdempotencyKey = idempotencyKey;
        Account = account;
        ShippingAddress = shippingAddress;
        HasFreeShippingCoupon = hasFreeShippingCoupon;
        ShippingFee = shippingFee;
        Total = total;
        TotalWithDiscount = totalWithDiscount;
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public void AddAppliedCoupon(AppliedCoupon appliedCoupon)
    {
        _appliedCoupons.Add(appliedCoupon);
    }
}
