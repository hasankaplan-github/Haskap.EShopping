using Haskap.EShopping.Domain.Shared.Enums;

namespace Haskap.EShopping.Domain.Shared.Consts;

public class CurrencyConverterConsts
{
    public const string PassThroughCurrencyConverterKey = "PassThrough";
    public const string AltinInCurrencyConverterKey = "AltinIn";
    public const string TcmbCurrencyConverterKey = "Tcmb";
    public const string FrankfurterApiCurrencyConverterKey = "FrankfurterApi";
    public const string PricingCurrencyConverterKey = "Pricing";

    public const Currency PassThroughCurrencyConverterDefaultCurrency = Currency.TRY;
    public const Currency PricingCurrencyConverterDefaultCurrency = Currency.TRY;
}
