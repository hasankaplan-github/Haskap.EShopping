using Haskap.DddBase.Domain;
using System.Net;

namespace Haskap.EShopping.Domain.Exceptions;
public class MoneyCurrenciesMismatchException : DomainException
{
    public MoneyCurrenciesMismatchException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
