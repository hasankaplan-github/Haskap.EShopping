using Haskap.DddBase.Domain;

namespace Modules.Account.Domain.AccountAggregate;
public class AccountRole : Entity
{
    private AccountRole() { }

    public AccountRole(Guid id)
        : base(id) { }

    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }
}
