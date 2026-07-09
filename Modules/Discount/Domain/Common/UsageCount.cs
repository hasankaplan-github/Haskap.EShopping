using Haskap.DddBase.Domain;

namespace Modules.Discount.Domain.Common;

public class UsageCount : ValueObject
{
    private readonly static Lock s_valueLock = new();
    public int Value { get; private set; }
    public int Limit { get; private set; }

    private UsageCount()
    {}

    public UsageCount(int limit)
    {
        Value = 0;
        Limit = limit;
    }

    public UsageCount Increment()
    {
        lock (s_valueLock)
        {
            if (Value >= Limit)
            {
                throw new InvalidOperationException("Usage count has reached its limit.");
            }

            return new UsageCount(Limit) { Value = Value + 1 };
        }
    }

    public bool CanIncrement()
    {
        lock (s_valueLock)
        {
            return Value < Limit;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Limit;
    }
}
