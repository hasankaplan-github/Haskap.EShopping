namespace Modules.Catalog.Domain.SizeAttributeAggregate;

public class SizeAttribute : AttributeBase
{
    public SizeAttribute(Guid id, string displayName, string value, string? description)
        : base(id, displayName, value, description)
    { }
}
