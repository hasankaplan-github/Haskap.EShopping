using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class SamePasswordException : DomainException
{
    public SamePasswordException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
