namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class CopyInputDto
{
    public Guid ProductIdToBeCopied { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
