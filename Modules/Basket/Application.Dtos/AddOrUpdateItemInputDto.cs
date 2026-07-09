namespace Modules.Basket.Application.Dtos;

public class AddOrUpdateItemInputDto
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
}
