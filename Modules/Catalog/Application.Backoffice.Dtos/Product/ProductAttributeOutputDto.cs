using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class ProductAttributeOutputDto
{
    public Guid Id { get; set; }
    public AttributeType AttributeType { get; set; }
    public string DisplayName { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}
