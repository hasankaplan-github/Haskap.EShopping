using Haskap.EShopping.Application.Dtos.ExchangeRates;
using Haskap.EShopping.Domain.Shared.Enums;

namespace Haskap.EShopping.Domain.Providers;

public interface ICurrencyConverter
{
    string Key { get; }
    decimal Convert(decimal fromAmount, Currency fromCurrency, Currency toCurrency);
    Task<IEnumerable<AvailableCurrencyOutputDto>> GetAvailableCurrenciesAsync(bool forceUpdate = false, CancellationToken cancellationToken = default);
    //Task<IEnumerable<ExchangeRateDto>> GetAllCurrenciesConvertedTo(Currency toCurrency, bool forceUpdate = false);
}