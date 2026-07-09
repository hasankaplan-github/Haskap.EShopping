using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.EShopping.Domain.Exceptions;
using Haskap.EShopping.Domain.Providers;
using Haskap.EShopping.Domain.Shared.Consts;
using Haskap.EShopping.Domain.Shared.Enums;

namespace Haskap.EShopping.Domain.Common;

[AddAuditHistoryLog]
public class Money : ValueObject
{
    public decimal Value { get; private set; }
    public Currency Currency { get; private set; }

    public static Money Zero => new Money(0);

    public Money(decimal value, Currency currency = Currency.TRY)
    {
        Value = value;
        Currency = currency;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Currency.GetSymbol()} {Value.ToString("#,##0.00")}";
    }

    public Money Convert(ICurrencyConverter currencyConverter, Currency toCurrency)
    {
        if (currencyConverter.Key == CurrencyConverterConsts.PassThroughCurrencyConverterKey)
        {
            toCurrency = Currency;
        }

        var convertedAmount = currencyConverter.Convert(Value, Currency, toCurrency);

        return new Money(convertedAmount, toCurrency);
    }

    public static Money SumUsingCurrencyConverter(ICurrencyConverter currencyConverter, IList<Money> moneys, Currency toCurrency)
    {
        if (currencyConverter.Key == CurrencyConverterConsts.PassThroughCurrencyConverterKey)
        {
            toCurrency = Currency._Invalid_;
        }

        var totalValue = moneys.Sum(x => currencyConverter.Convert(x.Value, x.Currency, toCurrency));

        return new Money(totalValue, toCurrency);
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new MoneyCurrenciesMismatchException();
        }

        return new Money(a.Value + b.Value, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new MoneyCurrenciesMismatchException();
        }

        return new Money(a.Value - b.Value, a.Currency);
    }

    public static Money operator *(decimal a, Money b)
    {
        return new Money(a * b.Value, b.Currency);
    }

    public static Money operator *(Money a, decimal b)
    {
        return b * a;
    }

    public static Money operator /(Money a, decimal b)
    {
        return new Money(a.Value / b, a.Currency);
    }

    public static Money operator -(Money a)
    {
        return new Money(-a.Value, a.Currency);
    }

    public static Money operator --(Money a)
    {
        var val = a.Value - 1;
        return new Money(val, a.Currency);
    }

    public static Money operator ++(Money a)
    {
        var val = a.Value + 1;
        return new Money(val, a.Currency);
    }
}
