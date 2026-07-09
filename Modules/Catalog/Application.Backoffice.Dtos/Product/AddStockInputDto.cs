namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class AddStockInputDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}
