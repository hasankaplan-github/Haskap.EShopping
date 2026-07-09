namespace Modules.Catalog.Domain.ColorAttributeAggregate;

public class ColorAttribute : AttributeBase
{
    public ColorAttribute(Guid id, string displayName, string value, string? description)
        : base(id, displayName, value, description)
    {}
}
