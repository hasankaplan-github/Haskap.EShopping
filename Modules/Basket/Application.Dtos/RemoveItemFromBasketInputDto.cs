namespace Modules.Basket.Application.Dtos;

public class RemoveItemFromBasketInputDto
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
}
