namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class CreateInputDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public IList<Guid>? CategoryIds { get; set; }
}
