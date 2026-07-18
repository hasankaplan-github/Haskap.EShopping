using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Basket.Domain.BasketAggregate;

public class BasketItem : Entity
{
    private static readonly Lock s_quantityLock = new();

    public Guid BasketId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public int Quantity { get; private set; }

    private BasketItem()
    { }

    internal BasketItem(Guid id, Guid productId, Guid productVariantId, int quantity)
        : base(id)
    {
        ProductId = productId;
        ProductVariantId = productVariantId;
        SetQuantity(quantity);
    }

    internal void SetQuantity(int newQuantity)
    {
        Guard.Against.NegativeOrZero(newQuantity, nameof(newQuantity));

        lock (s_quantityLock)
        {
            Quantity = newQuantity;
        }
    }
}
