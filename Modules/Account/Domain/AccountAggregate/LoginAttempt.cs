using Haskap.DddBase.Domain;

namespace Modules.Account.Domain.AccountAggregate;
public class LoginAttempt : ValueObject
{
    public const int MaxAllowedFailedAttemptCount = 3;
    private readonly TimeSpan _cooldownPeriod = TimeSpan.FromDays(3);

    public int FailedAttemptCount { get; private set; }
    public DateTime? LastFailedAttemptUtcDateTime { get; private set; }

    public LoginAttempt()
    {
        FailedAttemptCount = 0;
        LastFailedAttemptUtcDateTime = null;
    }

    public void SetFailedAttempt()
    {
        FailedAttemptCount++;
        LastFailedAttemptUtcDateTime = DateTime.UtcNow;
    }

    public bool ShouldLockAccount()
    {
        return FailedAttemptCount >= MaxAllowedFailedAttemptCount;
    }

    public void Reset()
    {
        FailedAttemptCount = 0;
        LastFailedAttemptUtcDateTime = null;
    }

    public bool HasExceededCooldownPeriod(TimeSpan cooldownPeriod)
    {
        if (LastFailedAttemptUtcDateTime == null)
        {
            return true;
        }

        return DateTime.UtcNow - LastFailedAttemptUtcDateTime.Value > cooldownPeriod;
    }

    public bool ShouldResetFailedAttempts()
    {
        if (LastFailedAttemptUtcDateTime == null || FailedAttemptCount < 1)
        {
            return false;
        }

        return HasExceededCooldownPeriod(_cooldownPeriod);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FailedAttemptCount;
        yield return LastFailedAttemptUtcDateTime ?? DateTime.MinValue;
    }
}
