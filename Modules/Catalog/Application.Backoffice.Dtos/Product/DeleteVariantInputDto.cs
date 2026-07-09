namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class DeleteVariantInputDto
{
    public Guid VariantId { get; set; }
    public Guid ProductId { get; set; }
}
