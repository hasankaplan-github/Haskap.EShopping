using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Shipping.Domain.NeighborhoodAggregate;

public class Neighborhood : AggregateRoot
{
    public Guid DistrictId { get; private set; }
    public string Name { get; private set; }

    private Neighborhood()
    {}

    public Neighborhood(Guid id, string name)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
    }
}
