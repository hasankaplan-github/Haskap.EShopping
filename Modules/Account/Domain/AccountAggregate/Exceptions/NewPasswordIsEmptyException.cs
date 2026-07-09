using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class NewPasswordIsEmptyException : DomainException
{
    public NewPasswordIsEmptyException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
