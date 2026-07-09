namespace Modules.Catalog.Application.Backoffice.Dtos.Category;

public class OutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SlugValue { get; set; }
    public bool IsActive { get; set; }
}
