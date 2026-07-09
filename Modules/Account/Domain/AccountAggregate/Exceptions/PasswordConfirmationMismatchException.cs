using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class PasswordConfirmationMismatchException : DomainException
{
    public PasswordConfirmationMismatchException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
