using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Modules.Catalog.Domain.ProductAggregate;

public class Sku : ValueObject
{
    public string Value { get; private set; }

    private Sku()
    {}

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Sku Generate(string text)
    {
        Guard.Against.NullOrWhiteSpace(text, nameof(text), "SKU cannot be null or whitespace.");

        //Value = $"{text.ToUpper().Replace(" ", "-")}-{Guid.NewGuid().ToString().Substring(0, 8)}";

        var skuValue = RemoveAccent(text.ToUpper());

        // invalid chars           
        skuValue = Regex.Replace(skuValue, @"[^A-Z0-9\s-_]", "");

        // convert multiple spaces into one space   
        skuValue = Regex.Replace(skuValue, @"\s+", " ").Trim();

        // convert spaces into hyphens
        skuValue = Regex.Replace(skuValue, @"\s", "-");

        //skuValue = Regex.Replace(skuValue, "[aeiou]", "", RegexOptions.IgnoreCase);

        return new()
        {
            Value = skuValue
        };
    }

    private static string RemoveAccent(string text)
    {
        //byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //return System.Text.Encoding.ASCII.GetString(bytes);

        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }
}
