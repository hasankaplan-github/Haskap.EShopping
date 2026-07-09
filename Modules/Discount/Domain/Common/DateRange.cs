using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Discount.Domain.Common;

public class DateRange : ValueObject
{
    public DateTime UtcStartDateTime { get; private set; }
    public DateTime UtcEndDateTime { get; private set; }

    private DateRange()
    {}

    public DateRange(DateTime utcStartDateTime, DateTime utcEndDateTime)
    {
        Guard.Against.OutOfRange(utcStartDateTime, nameof(utcStartDateTime), DateTime.MinValue, utcEndDateTime);
        Guard.Against.OutOfRange(utcEndDateTime, nameof(utcEndDateTime), utcStartDateTime, DateTime.MaxValue);

        UtcStartDateTime = utcStartDateTime;
        UtcEndDateTime = utcEndDateTime;
    }

    public bool IsInRange(DateTime utcDateTime)
    {
        return utcDateTime >= UtcStartDateTime && utcDateTime <= UtcEndDateTime;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UtcStartDateTime;
        yield return UtcEndDateTime;
    }
}
