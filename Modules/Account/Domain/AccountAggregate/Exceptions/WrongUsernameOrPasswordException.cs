using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class WrongUsernameOrPasswordException : DomainException
{
    public WrongUsernameOrPasswordException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
