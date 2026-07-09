using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.AccountAggregate.Exceptions;
public class AccountIsLockedException : DomainException
{
    public AccountIsLockedException()
        : base(HttpStatusCode.Locked)
    {

    }
}
