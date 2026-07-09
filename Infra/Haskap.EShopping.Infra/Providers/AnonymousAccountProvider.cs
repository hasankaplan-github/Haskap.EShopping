using Haskap.EShopping.Domain.Providers;

namespace Haskap.EShopping.Infra.Providers;

public class AnonymousAccountProvider : IAnonymousAccountProvider
{
    public Guid AccountId { get; set; }
}
