using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;

namespace Modules.Catalog.Domain.CategoryAggregate;

public class Category : AggregateRoot, IIsActive
{
    public string Name { get; private set; }
    public Slug Slug { get; private set; }
    public bool IsActive { get; set; }
    

    private Category()
    {}

    public Category(Guid id, string name, bool isActive)
        : base(id)
    {
        SetName(name);
        IsActive = isActive;
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        Slug = Slug.Generate(name, 20);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
