namespace Modules.Basket.Application.Dtos;

public class AddJustOneItemInputDto
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
}
