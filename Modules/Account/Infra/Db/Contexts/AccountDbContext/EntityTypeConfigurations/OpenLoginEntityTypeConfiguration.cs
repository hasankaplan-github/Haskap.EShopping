using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Account.Domain.AccountAggregate;

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.EntityTypeConfigurations;

public class OpenLoginEntityTypeConfiguration : BaseEntityTypeConfiguration<OpenLogin>
{
    public override void Configure(EntityTypeBuilder<OpenLogin> builder)
    {
        base.Configure(builder);

        builder.HasOne<Domain.AccountAggregate.Account>()
            .WithMany(x => x.OpenLogins)
            .HasForeignKey(x => x.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.AccountId, x.Id });
    }
}
