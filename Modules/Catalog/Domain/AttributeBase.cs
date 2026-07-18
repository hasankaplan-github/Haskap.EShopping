using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.Catalog.Domain;

public class AttributeBase : AggregateRoot
{
    public string DisplayName { get; private set; }
    public string Value { get; private set; }
    public string? Description { get; set; }

    private AttributeBase()
    {}

    public AttributeBase(Guid id, string displayName, string value, string? description)
       : base(id)
    {
        SetDisplayName(displayName);
        SetValue(value);
        Description = description;
    }

    public void SetDisplayName(string displayName)
    {
        Guard.Against.NullOrWhiteSpace(displayName, nameof(displayName));
        DisplayName = displayName;
    }

    public void SetValue(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        Value = value;
    }
}
