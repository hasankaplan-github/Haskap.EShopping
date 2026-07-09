using Haskap.EShopping.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Order.Domain.OrderAggregate;

public class Item : Entity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; }
    public string VariantSkuValue { get; private set; }
    public string SlugValue { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }
    public Money Total { get; private set; }
    public Money TotalWithDiscount { get; private set; }

    private Item()
    {}

    public Item(
        Guid id,
        Guid productId,
        Guid variantId,
        string productName,
        string variantSkuValue,
        string slugValue,
        Money price,
        int quantity,
        Money total,
        Money totalWithDiscount)
        : base(id)
    {
        ProductId = productId;
        VariantId = variantId;
        ProductName = productName;
        VariantSkuValue = variantSkuValue;
        SlugValue = slugValue;
        Price = price;
        Quantity = quantity;
        Total = total;
        TotalWithDiscount = totalWithDiscount;
    }
}
