using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Account.Domain.RoleAggregate.Exceptions;
public class DuplicateRoleNameException : DomainException
{
    public DuplicateRoleNameException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
