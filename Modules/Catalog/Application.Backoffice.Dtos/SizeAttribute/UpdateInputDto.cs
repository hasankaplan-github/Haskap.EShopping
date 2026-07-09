namespace Modules.Catalog.Application.Backoffice.Dtos.SizeAttribute;

public class UpdateInputDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}
