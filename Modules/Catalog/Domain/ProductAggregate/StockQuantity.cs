using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Catalog.Domain.ProductAggregate;

public class StockQuantity : ValueObject
{
    public int Value { get; private set; }

    private StockQuantity()
    {}

    public StockQuantity(int value)
    {
        Guard.Against.Negative(value, nameof(value), "Stock quantity cannot be negative.");

        Value = value;
    }

    public bool IsAvailable(int requestedQuantity)
    {
        Guard.Against.Negative(requestedQuantity, nameof(requestedQuantity), "Requested quantity cannot be negative.");
        return Value >= requestedQuantity;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
