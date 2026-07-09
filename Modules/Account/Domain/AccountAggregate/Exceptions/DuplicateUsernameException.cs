using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class DuplicateUsernameException : DomainException
{
    public DuplicateUsernameException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
