using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class CurrentPasswordIsEmptyException : DomainException
{
    public CurrentPasswordIsEmptyException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
