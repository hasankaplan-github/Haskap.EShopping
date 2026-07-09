using Haskap.DddBase.Domain;
using System.Security.Cryptography;
using System.Text;

namespace Modules.Order.Domain.OrderAggregate;

public class Code : ValueObject
{
    private static readonly string[] s_numbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    private static readonly string[] s_upperCaseLetters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
    private static string[] s_allCharacters = [.. s_numbers, .. s_upperCaseLetters];

    public string Value { get; private set; }

    private Code()
    {}

    public static Code Generate()
    {
        var codeValue = new StringBuilder();

        RandomNumberGenerator.Shuffle<string>(s_allCharacters);

        for (int i = 0; i < 8; i++)
        {
            codeValue.Append(s_allCharacters[RandomNumberGenerator.GetInt32(s_allCharacters.Length)]);
        }

        return new()
        {
            Value = $"{DateTime.UtcNow:yyyyMMddHHmm}{codeValue.ToString()}"
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
