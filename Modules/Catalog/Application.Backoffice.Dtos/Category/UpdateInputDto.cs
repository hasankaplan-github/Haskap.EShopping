namespace Modules.Catalog.Application.Backoffice.Dtos.Category;

public class UpdateInputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}
