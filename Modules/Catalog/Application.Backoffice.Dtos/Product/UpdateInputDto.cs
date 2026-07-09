namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class UpdateInputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Guid>? CategoryIds { get; set; }
    public bool IsActive { get; set; }
}
