using Ardalis.GuardClauses;
using Haskap.EShopping.Domain;
using Modules.Shipping.Domain.NeighborhoodAggregate;

namespace Modules.Shipping.Domain.DistrictAggregate;

public class District : AggregateRoot
{
    public Guid CityId { get; private set; }
    public string Name { get; private set; }
    public IReadOnlyList<Neighborhood> Neighborhoods => _neighborhoods.AsReadOnly();
    private List<Neighborhood> _neighborhoods = [];

    private District()
    { }

    public District(Guid id, string name)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
    }
}
