using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Modules.Catalog.Domain.Shared.Enums;
using NpgsqlTypes;

namespace Modules.Catalog.Domain.ProductAggregate;

public class ProductAttribute : ValueObject
{
    public Guid Id { get; init; }
    public AttributeType AttributeType { get; private set; }
    public string DisplayName { get; private set; }
    public string Value { get; private set; }
    public string? Description { get; private set; }
    public NpgsqlTsVector SearchVectorTurkish { get; set; }

    private ProductAttribute()
    {}

    public ProductAttribute(AttributeType attributeType, string displayName, string value, string? description)
    {
        Guard.Against.NullOrWhiteSpace(displayName, nameof(displayName));
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        AttributeType = attributeType;
        DisplayName = displayName;
        Value = value;
        Description = description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AttributeType;
        yield return DisplayName;
        yield return Value;
        yield return Description ?? string.Empty;
    }
}
