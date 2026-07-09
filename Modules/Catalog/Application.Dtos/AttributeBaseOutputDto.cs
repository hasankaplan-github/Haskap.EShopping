namespace Modules.Catalog.Application.Dtos;

public class AttributeBaseOutputDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}
