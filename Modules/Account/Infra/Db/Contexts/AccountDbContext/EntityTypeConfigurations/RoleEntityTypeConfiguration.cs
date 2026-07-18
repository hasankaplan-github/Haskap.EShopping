using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Account.Domain.RoleAggregate;
using Modules.Account.Domain.Shared.Consts;

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.EntityTypeConfigurations;

public class RoleEntityTypeConfiguration : BaseEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .HasMaxLength(RoleConsts.MaxNameLength);

        builder.OwnsMany(x => x.Permissions, x =>
        {
            x.WithOwner().HasForeignKey("RoleId");
            x.HasKey(x => x.Id);
        });
    }
}
