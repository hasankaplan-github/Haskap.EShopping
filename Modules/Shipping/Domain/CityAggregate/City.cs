using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Modules.Shipping.Domain.DistrictAggregate;

namespace Modules.Shipping.Domain.CityAggregate;

public class City : AggregateRoot
{
    public string Name { get; private set; }
    public IReadOnlyList<District> Districts => _districts.AsReadOnly();
    private List<District> _districts = [];

    private City()
    { }

    public City(Guid id, string name)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
    }
}
