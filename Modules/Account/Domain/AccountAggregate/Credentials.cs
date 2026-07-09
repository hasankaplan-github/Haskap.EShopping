using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Account.Domain.AccountAggregate;
public class Credentials : ValueObject
{
    public string Username { get; private set; }
    public Password Password { get; private set; }

    private Credentials()
    {
    }

    public Credentials(string username, Password password)
    {
        Guard.Against.NullOrWhiteSpace(username, nameof(username), "Kullanıcı Adı boş olamaz!");
        Guard.Against.Null(password);

        Username = username;
        Password = password;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return Password;
    }
}
