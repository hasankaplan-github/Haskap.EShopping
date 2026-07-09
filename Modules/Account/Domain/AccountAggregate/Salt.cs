using System.Security.Cryptography;
using Haskap.DddBase.Domain;

namespace Modules.Account.Domain.AccountAggregate;
public class Salt : ValueObject
{
    public string Value { get; private set; }

    private Salt()
    {}

    public static Salt Generate(int lengthInBytes = 16)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(lengthInBytes);
        var saltValue = Convert.ToBase64String(saltBytes);

        return new Salt
        {
            Value = saltValue
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
