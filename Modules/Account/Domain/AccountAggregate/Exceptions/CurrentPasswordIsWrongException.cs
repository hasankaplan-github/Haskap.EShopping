using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class CurrentPasswordIsWrongException : DomainException
{
    public CurrentPasswordIsWrongException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
