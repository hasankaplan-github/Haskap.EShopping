using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Account.Domain.Shared.Consts;

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.EntityTypeConfigurations;

public class AccountEntityTypeConfiguration : BaseEntityTypeConfiguration<Domain.AccountAggregate.Account>
{
    public override void Configure(EntityTypeBuilder<Domain.AccountAggregate.Account> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.FirstName)
            .HasMaxLength(AccountConsts.MaxFirstNameLength);

        builder.Property(x => x.LastName)
            .HasMaxLength(AccountConsts.MaxLastNameLength);

        builder.OwnsOne(a => a.Credentials, x =>
        {
            x.OwnsOne(y => y.Password, y =>
            {
                y.OwnsOne(z => z.Salt);
                y.Ignore(z => z.ClearValue);
            });
            x.HasIndex(y => y.Username);
        });

        builder.OwnsMany(x => x.Permissions, x =>
        {
            x.WithOwner().HasForeignKey("AccountId");
            x.HasKey(y => y.Id);
        });

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.IsLocked)
            .HasDefaultValue(false);

        builder.OwnsOne(x => x.LoginAttempt, x =>
        {
            x.Property(y => y.FailedAttemptCount)
                .HasDefaultValue(0);

            x.Property(y => y.LastFailedAttemptUtcDateTime)
                .HasDefaultValue(null);
        });
    }
}
