namespace Modules.Catalog.Application.Dtos;

public class AddToBasketInputDto
{
    public Guid ProductId { get; set; }
    public Guid ProductVariantId { get; set; }
}
