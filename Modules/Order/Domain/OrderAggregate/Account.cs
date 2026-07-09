using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Order.Domain.OrderAggregate;

public class Account : ValueObject
{
    public Guid? OwnerAccountId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string EmailAddress { get; private set; }

    private Account()
    {}

    public Account(Guid? ownerAccountId, string firstName, string lastName, string phoneNumber, string emailAddress)
    {
        Guard.Against.NullOrWhiteSpace(firstName, message: "İsim boş olamaz!");
        Guard.Against.NullOrWhiteSpace(lastName, message: "Soyisim boş olamaz!");
        Guard.Against.NullOrWhiteSpace(phoneNumber, message: "Telefon Numarası boş olamaz!");
        Guard.Against.NullOrWhiteSpace(emailAddress, message: "Eposta Adresi boş olamaz!");

        OwnerAccountId = ownerAccountId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return PhoneNumber;
        yield return EmailAddress;
    }
}
