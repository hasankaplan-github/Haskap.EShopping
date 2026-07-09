using Haskap.DddBase.Domain;
using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Domain.ProductAggregate;

public class ProductAttributeTypeForDetails : ValueObject
{
    public Guid Id { get; init; }
    public AttributeType AttributeType { get; private set; }

    private ProductAttributeTypeForDetails()
    {}

    public ProductAttributeTypeForDetails(AttributeType attributeType)
    {
        AttributeType = attributeType;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AttributeType;
    }
}
