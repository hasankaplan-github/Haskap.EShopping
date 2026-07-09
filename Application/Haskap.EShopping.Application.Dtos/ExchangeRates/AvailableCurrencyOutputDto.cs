using Haskap.EShopping.Domain.Shared.Enums;

namespace Haskap.EShopping.Application.Dtos.ExchangeRates;
public class AvailableCurrencyOutputDto
{
    public Currency Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencyEnglishName { get; set; }

    public override string ToString()
    {
        return Currency.ToString() +
            (!string.IsNullOrWhiteSpace(CurrencyName) ? " - " + CurrencyName : string.Empty) +
            (!string.IsNullOrWhiteSpace(CurrencyEnglishName) ? " - " + CurrencyEnglishName : string.Empty);

    }
}
