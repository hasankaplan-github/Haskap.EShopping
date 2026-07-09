using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Domain.AccountAggregate;
using Modules.Account.Domain.RoleAggregate;

namespace Modules.Account.Domain;
public interface IAccountDbContext : IUnitOfWork
{
    DbSet<AccountAggregate.Account> Account { get; set; }
    DbSet<Role> Role { get; set; }
    DbSet<AccountRole> AccountRole { get; set; }
    DbSet<OpenLogin> OpenLogin { get; set; }
}
