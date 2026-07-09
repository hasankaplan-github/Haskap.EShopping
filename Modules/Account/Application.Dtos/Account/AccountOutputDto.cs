namespace Modules.Account.Application.Dtos.Account;
public class AccountOutputDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsLocked { get; set; }
    public int FailedAttemptCount { get; set; }
    public DateTime? LastFailedAttemptUtcDateTime { get; set; }
}
