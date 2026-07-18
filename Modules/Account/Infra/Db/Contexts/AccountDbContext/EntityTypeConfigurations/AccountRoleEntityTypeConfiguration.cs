using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Account.Domain.AccountAggregate;
using Modules.Account.Domain.RoleAggregate;

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.EntityTypeConfigurations;

public class AccountRoleEntityTypeConfiguration : BaseEntityTypeConfiguration<AccountRole>
{
    public override void Configure(EntityTypeBuilder<AccountRole> builder)
    {
        base.Configure(builder);

        builder.HasOne<Domain.AccountAggregate.Account>()
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        //builder.HasIndex(x => new { x.UserId, x.RoleId });
    }
}
