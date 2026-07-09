namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class EmptyStockInputDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
}
