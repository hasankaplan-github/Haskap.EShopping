using System.Collections.Concurrent;
using System.Globalization;

namespace Haskap.EShopping.Domain.Shared.Enums;
public static class CurrencyExtensionMethods
{
    private static readonly Lock s_symbolCachingLock = new();
    private static readonly ConcurrentDictionary<Currency, string> s_currencySymbolMap = new ConcurrentDictionary<Currency, string>();

    public static string GetSymbol(this Currency currency)
    {
        var symbol = string.Empty;

        if(!s_currencySymbolMap.TryGetValue(currency, out symbol))
        {
            lock (s_symbolCachingLock)
            {
                if (s_currencySymbolMap.TryGetValue(currency, out symbol))
                {
                    return symbol;
                }

                var region = CultureInfo
                    .GetCultures(CultureTypes.SpecificCultures)
                    .Select(x => new RegionInfo(x.Name))
                    .Where(r => r.ISOCurrencySymbol == currency.ToString())
                    .FirstOrDefault();

                symbol = region?.CurrencySymbol ?? string.Empty;
                s_currencySymbolMap.AddOrUpdate(
                    currency,
                    symbol,
                    (_, _) => symbol);
            }
        }

        return symbol;
    }
}
